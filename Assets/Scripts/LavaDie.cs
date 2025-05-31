using UnityEngine;

public class LavaDie : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (!other.transform.CompareTag("Player")) { return; }

        other.transform.GetComponent<RespawnSystem>()?.Die();
    }
}
