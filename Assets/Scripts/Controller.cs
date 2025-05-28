using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class Controller : MonoBehaviour
{
    public float speed = 10f;
    public float mouseSens = 100f;
    [SerializeField] private Vector2 maxMinRot = new Vector2(-90, 90);
    [SerializeField] private Camera cam;
    private CharacterController characterController;
    private Vector3 currentRotation;

    private Vector3 CharPos => characterController.transform.position;

    private void Awake()
    {
        characterController = GetComponent<CharacterController>();
    }

    private void Update()
    {
        Move();
    }

    private void Move()
    {
        var x = Input.GetAxis("Horizontal");
        var y = Input.GetAxis("Vertical");
        Vector3 moveVector = new Vector3(x, 0.0f, y);

        if (moveVector.magnitude <= 0.1f) { return; }

        float rotAngle = Mathf.Atan2(moveVector.x, moveVector.z) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0f, rotAngle, 0f);

        Vector3 moveDir = Quaternion.Euler(0f, rotAngle, 0f) * Vector3.forward;

        // Применяем движение через CharacterController
        characterController.Move(moveDir.normalized * speed * Time.deltaTime);

        characterController.Move(moveVector.magnitude <= 1 ?
            Time.deltaTime * speed * moveVector : // Если ленейно WASD
            Time.deltaTime * speed * moveVector.normalized); // Если W+D; W+A; S+D; S+A;


    }

    //private void CamMove()
    //{
    //    Vector3 camVec = new Vector3(CharPos.x, CharPos.y + 2.2f, CharPos.z - 3.8f);
    //    cam.transform.position = camVec;
    //}
}
