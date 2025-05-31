using UnityEngine;

public class LiftScript : MonoBehaviour
{
    public float speed = 2f;
    [SerializeField] private bool x;
    [SerializeField] private bool y;


    [SerializeField] private Transform point1;
    public bool stayOn1 = true;
    [SerializeField] private Transform point2;
    public bool stayOn2 = false;

    public Vector3 Direction1 { get; private set; } = new(0, 0, 0);
    public Vector3 Direction2 { get; private set; } = new(0, 0, 0);

    [SerializeField] private bool isCurve = false;
    [SerializeField] private Transform[] curvePoints;

    private void Awake()
    {
        Direction1 = (point1.position - point2.position).normalized;
        Direction2 = (point2.position - point1.position).normalized;
    }

    private void Update()
    {
        if (isCurve) { return; }
        Vector3 direction = new Vector3(0, 0, 0);
        if (stayOn1)
        {
            direction = Direction1;
        }
        else if (stayOn2)
        {
            direction = Direction2;
        }


        if ((transform.position - point1.position).magnitude <= 1f)
        {
            stayOn1 = true;
            stayOn2 = false;
        }
        else if ((transform.position - point2.position).magnitude <= 1f)
        {
            stayOn1 = false;
            stayOn2 = true;
        }
        
        transform.position -= Time.deltaTime * speed * direction;
    }
}
