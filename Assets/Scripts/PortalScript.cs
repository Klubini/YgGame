using UnityEngine;
using YG;

public class PortalScript : MonoBehaviour
{
    [SerializeField] private CharacterController player;
    [SerializeField] private Transform spawnPoint;


    private void OnTriggerEnter(Collider other)
    {
        if (YG2.saves.currentlvl >= 50)
        {
            YG2.saves.isSecondPart = true;
            YG2.SaveProgress();
            player.enabled = false;
            player.transform.position = spawnPoint.position;
            player.enabled = true;
        }
    }
}
