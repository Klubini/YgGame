using UnityEngine;

public class FrameLim : MonoBehaviour
{
    [SerializeField] int fps;

    private void OnValidate()
    {
        Application.targetFrameRate = fps;
    }
}
