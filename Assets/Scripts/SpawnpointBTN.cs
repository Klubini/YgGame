using UnityEngine;
using YG;

public class SpawnpointBTN : MonoBehaviour
{
    [SerializeField] private SpawnPoints logic;
    [SerializeField] private Animator flag;
    [SerializeField] private ParticleSystem particle;
    [SerializeField] private RespawnSystem respawn;
    private bool hasBeenTriggered = false;


    private void Start()
    {
        if (YG2.saves.collects[logic.locationIndex - 1])
        {
            logic.saveWasGetted = true;
            flag.SetBool("getted", true);
            ProgressScript.progressNumBaseLVL++;
            respawn.Die();
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (!other.transform.CompareTag("Player") || logic.saveWasGetted || hasBeenTriggered) { return; }
        hasBeenTriggered = true;
        particle.Play();

        logic.saveWasGetted = true;
        flag.SetBool("getted", true);
        ProgressScript.progressNumBaseLVL++;
        YG2.saves.collects[logic.locationIndex - 1] = true;
        YG2.saves.currentlvl = logic.locationIndex;
        YG2.SaveProgress();
    }
}
