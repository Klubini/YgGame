using UnityEngine;

public class SpawnpointBTN : MonoBehaviour
{
    [SerializeField] private SpawnPoints logic;
    [SerializeField] private Animator flag;
    [SerializeField] private ParticleSystem particle;
    private bool hasBeenTriggered = false;

    private void OnTriggerEnter(Collider other)
    {
        if ((!other.transform.CompareTag("Player") && !logic.saveWasGetted) || hasBeenTriggered) { return; }
        hasBeenTriggered = true;
        particle.Play();

        logic.saveWasGetted = true;
        flag.SetBool("getted", true);
        ProgressScript.progressNumBaseLVL++;
    }
}
