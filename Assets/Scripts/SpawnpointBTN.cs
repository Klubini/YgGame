using UnityEngine;

public class SpawnpointBTN : MonoBehaviour
{
    [SerializeField] private SpawnPoints logic;
    [SerializeField] private Animator flag;
    private bool hasBeenTriggered = false;

    private void OnTriggerEnter(Collider other)
    {
        if ((!other.transform.CompareTag("Player") && !logic.saveWasGetted) || hasBeenTriggered) { return; }
        hasBeenTriggered = true;

        logic.saveWasGetted = true;
        flag.SetBool("getted", true);
        ProgressScript.progressNumBaseLVL++;
    }
}
