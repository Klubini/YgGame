using UnityEngine;

public class SpawnPoints : MonoBehaviour
{
    public bool isSecondPart = false;
    public int locationIndex = 0;
    public bool saveWasGetted = false;
    [SerializeField] private Transform spawnPoint;

    public void Spawn(CharacterController player)
    {
        Vector3 spawnPos = new Vector3(spawnPoint.position.x, spawnPoint.position.y, spawnPoint.position.z);
        player.enabled = false;
        player.transform.position = spawnPos;
        player.enabled = true;
    }
}
