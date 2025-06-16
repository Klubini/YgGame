using UnityEngine;
using UnityEngine.InputSystem;
using YG;

public class JoyStickADAPTATION : MonoBehaviour
{
    void Update()
    {
        if (YG2.envir.device == YG2.Device.Desktop)
        {
            gameObject.SetActive(false);
        }
    }
}
