
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

    private float mouseX;
    private float mouseY;
    private float offsetDistanceY;
    private Transform player;
    private Vector2 previousTouchPosition;
    private float initialFingersDistance;
    private float initialFOV;

    void Start()
    {
        player = GameObject.FindWithTag("Player").transform;
        offsetDistanceY = transform.position.y;

        if (!clickToMoveCamera && Application.isEditor)
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

        // Для мобильных - пинч-жест
        if (Input.touchCount == 2)
        {
            Touch touch1 = Input.GetTouch(0);
            Touch touch2 = Input.GetTouch(1);

            if (touch1.phase == TouchPhase.Began || touch2.phase == TouchPhase.Began)
            {
                initialFingersDistance = Vector2.Distance(touch1.position, touch2.position);
                initialFOV = Camera.main.fieldOfView;
            }
            else if (touch1.phase == TouchPhase.Moved || touch2.phase == TouchPhase.Moved)
            {
                float currentFingersDistance = Vector2.Distance(touch1.position, touch2.position);
                float zoomFactor = initialFingersDistance / currentFingersDistance;
                Camera.main.fieldOfView = Mathf.Clamp(initialFOV * zoomFactor, fovLimits.x, fovLimits.y);
            }
        }
    }

    private void HandleRotation()
    {
        bool shouldRotate = false;

        // Для ПК - ПКМ или ЛКМ в области
        if (!Application.isMobilePlatform)
        {
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
        }
        // Для мобильных - тач в области
        else if (Input.touchCount == 1)
        {
            Touch touch = Input.GetTouch(0);
            
            if (touch.phase == TouchPhase.Moved && rotationArea != null)
            {
                if (IsPointerInRotationArea(touch.position) && (!ignoreUI || !IsPointerOverUIElement(touch.fingerId)))
                {
                    shouldRotate = true;
                    previousTouchPosition = touch.position;
                }
            }
        }

        if (shouldRotate)
        {
            Vector2 delta = Vector2.zero;
            
            if (Application.isMobilePlatform && Input.touchCount == 1)
            {
                Touch touch = Input.GetTouch(0);
                delta = touch.position - previousTouchPosition;
                previousTouchPosition = touch.position;
            }
            else
            {
                delta = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
            }

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
}