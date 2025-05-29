using UnityEngine;

public class SpawnpointBTN : MonoBehaviour
{
    [SerializeField] private SpawnPoints logic;
    [SerializeField] private Animator flag;


    private void OnTriggerEnter(Collider other)
    {
        if (!other.transform.CompareTag("Player") && !logic.saveWasGetted) { return; }

        logic.saveWasGetted = true;
        flag.SetBool("getted", true);
    }
}
