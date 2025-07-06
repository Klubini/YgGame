using UnityEngine;
using UnityEngine.EventSystems;

public class CameraController : MonoBehaviour
{
    [Header("Основные настройки")]
    [Tooltip("Включить вращение при зажатии ПКМ (только для ПК)")]
    public bool clickToMoveCamera = false;
    [Tooltip("Включить зум (колесом мыши или пинч-жестом)")]
    public bool canZoom = true;
    [Space]
    [Tooltip("Чувствительность вращения")]
    public float sensitivity = 5f;
    [Tooltip("Чувствительность зума")]
    public float zoomSensitivity = 2f;

    [Header("Ограничения")]
    [Tooltip("Ограничения по вертикали (min/max)")]
    public Vector2 cameraLimit = new Vector2(-45, 40);
    [Tooltip("Минимальное и максимальное FOV")]
    public Vector2 fovLimits = new Vector2(20, 80);

    [Header("Область вращения (для мобильных)")]
    [Tooltip("RectTransform области для вращения")]
    public RectTransform rotationArea;
    [Tooltip("Игнорировать UI элементы при вращении")]
    public bool ignoreUI = true;

    [Header("Джойстики (для мобильных)")]
    [Tooltip("RectTransform джойстиков, которые не должны влиять на камеру")]
    public RectTransform[] joysticks;

    private float mouseX;
    private float mouseY;
    private float offsetDistanceY;
    private Transform player;
    private Vector2 previousTouchPosition;
    private float initialFingersDistance;
    private float initialFOV;
    private bool isRotating = false;
    private int? rotationFingerId = null; // ID пальца для вращения

    void Start()
    {
        player = GameObject.FindWithTag("Player").transform;
        offsetDistanceY = transform.position.y;

        if (!clickToMoveCamera && Application.isEditor && !Application.isMobilePlatform)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }

    void Update()
    {
        transform.position = player.position + new Vector3(0, offsetDistanceY, 0);

        HandleZoom();
        HandleRotation();
    }

    private void HandleZoom()
    {
        if (!canZoom) return;

        // Для ПК - колесо мыши
        if (Input.GetAxis("Mouse ScrollWheel") != 0)
        {
            Camera.main.fieldOfView -= Input.GetAxis("Mouse ScrollWheel") * zoomSensitivity * 10;
            Camera.main.fieldOfView = Mathf.Clamp(Camera.main.fieldOfView, fovLimits.x, fovLimits.y);
        }

        // Для мобильных - пинч-жест (только если нет активного вращения)
        // if (Input.touchCount == 2 && !isRotating)
        // {
        //     Touch touch1 = Input.GetTouch(0);
        //     Touch touch2 = Input.GetTouch(1);

        //     // Пропускаем касания джойстиков
        //     if (IsJoystickTouch(touch1.position) || IsJoystickTouch(touch2.position))
        //         return;

        //     if (touch1.phase == TouchPhase.Began || touch2.phase == TouchPhase.Began)
        //     {
        //         initialFingersDistance = Vector2.Distance(touch1.position, touch2.position);
        //         initialFOV = Camera.main.fieldOfView;
        //     }
        //     else if (touch1.phase == TouchPhase.Moved || touch2.phase == TouchPhase.Moved)
        //     {
        //         float currentFingersDistance = Vector2.Distance(touch1.position, touch2.position);
        //         float zoomFactor = initialFingersDistance / currentFingersDistance;
        //         Camera.main.fieldOfView = Mathf.Clamp(initialFOV * zoomFactor, fovLimits.x, fovLimits.y);
        //     }
        // }
    }

    private void HandleRotation()
    {
        if (Application.isMobilePlatform)
        {
            HandleMobileRotation();
        }
        else
        {
            HandlePCRotation();
        }
    }

    private void HandleMobileRotation()
    {
        // Сбрасываем вращение, если палец убран
        if (rotationFingerId.HasValue)
        {
            bool fingerStillActive = false;
            foreach (Touch touch in Input.touches)
            {
                if (touch.fingerId == rotationFingerId.Value)
                {
                    fingerStillActive = true;
                    break;
                }
            }
            
            if (!fingerStillActive)
            {
                isRotating = false;
                rotationFingerId = null;
            }
        }

        // Обрабатываем активное вращение
        if (isRotating && rotationFingerId.HasValue)
        {
            Touch? rotationTouch = null;
            foreach (Touch touch in Input.touches)
            {
                if (touch.fingerId == rotationFingerId.Value)
                {
                    rotationTouch = touch;
                    break;
                }
            }

            if (rotationTouch.HasValue)
            {
                Touch touch = rotationTouch.Value;
                Vector2 delta = touch.position - previousTouchPosition;
                previousTouchPosition = touch.position;

                mouseX += delta.x * sensitivity * Time.deltaTime * 60;
                mouseY -= delta.y * sensitivity * Time.deltaTime * 60;
                mouseY = Mathf.Clamp(mouseY, cameraLimit.x, cameraLimit.y);
                transform.rotation = Quaternion.Euler(mouseY, mouseX, 0);
            }
        }
        // Поиск нового касания для вращения
        else
        {
            foreach (Touch touch in Input.touches)
            {
                // Пропускаем касания джойстиков
                if (IsJoystickTouch(touch.position)) continue;
                
                // Игнорируем касания в UI
                if (ignoreUI && IsPointerOverUIElement(touch.fingerId)) continue;
                
                // Проверяем область вращения
                if (rotationArea != null && !IsPointerInRotationArea(touch.position)) continue;
                
                // Начинаем вращение при движении нового пальца
                if (touch.phase == TouchPhase.Began || touch.phase == TouchPhase.Moved)
                {
                    isRotating = true;
                    rotationFingerId = touch.fingerId;
                    previousTouchPosition = touch.position;
                    break; // Берем только одно касание для вращения
                }
            }
        }
    }

    private void HandlePCRotation()
    {
        bool shouldRotate = false;

        if (clickToMoveCamera && Input.GetAxisRaw("Fire2") != 0)
        {
            shouldRotate = true;
        }
        else if (rotationArea != null && Input.GetMouseButton(0))
        {
            if (IsPointerInRotationArea() && (!ignoreUI || !IsPointerOverUIElement()))
            {
                shouldRotate = true;
            }
        }

        if (shouldRotate)
        {
            Vector2 delta = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));

            mouseX += delta.x * sensitivity * Time.deltaTime * 60;
            mouseY -= delta.y * sensitivity * Time.deltaTime * 60;
            mouseY = Mathf.Clamp(mouseY, cameraLimit.x, cameraLimit.y);
            transform.rotation = Quaternion.Euler(mouseY, mouseX, 0);
        }
    }

    private bool IsPointerInRotationArea(Vector2 screenPosition = default)
    {
        if (rotationArea == null) return false;
        
        if (screenPosition == default)
            screenPosition = Input.mousePosition;

        return RectTransformUtility.RectangleContainsScreenPoint(
            rotationArea, 
            screenPosition, 
            null
        );
    }

    private bool IsPointerOverUIElement(int fingerId = -1)
    {
        if (fingerId >= 0)
            return EventSystem.current.IsPointerOverGameObject(fingerId);
        else
            return EventSystem.current.IsPointerOverGameObject();
    }

    // Проверка, находится ли касание в зоне джойстика
    private bool IsJoystickTouch(Vector2 position)
    {
        if (joysticks == null || joysticks.Length == 0) 
            return false;

        foreach (RectTransform joystick in joysticks)
        {
            if (joystick != null && 
                RectTransformUtility.RectangleContainsScreenPoint(joystick, position, null))
            {
                return true;
            }
        }
        return false;
    }
}