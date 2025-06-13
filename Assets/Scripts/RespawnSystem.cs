using UnityEngine;
using System.Collections.Generic;
using YG;

public class RespawnSystem : MonoBehaviour
{
    public List<SpawnPoints> spawnPoints;
    private CharacterController self;

    private void Awake()
    {
        self = GetComponent<CharacterController>();
    }

    private void Update()
    {
        if (transform.position.y <= -5)
        {
            Die();
        }
    }
    public void Die()
    {
        for (int i = 0; i < spawnPoints.Count; i++)
        {
            if (spawnPoints[i].saveWasGetted)
            {
                if (spawnPoints[i + 1] != null && !spawnPoints[i + 1].saveWasGetted)
                {
                    spawnPoints[i].Spawn(self);
                    return;
                }
            }
        }

        YG2.InterstitialAdvShow();

        self.enabled = false;
        self.transform.position = new Vector3(0, 2, -14);
        self.enabled = true;
    }  
}
