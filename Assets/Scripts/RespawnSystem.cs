using UnityEngine;
using System.Collections.Generic;

public class RespawnSystem : MonoBehaviour
{
    [SerializeField] private List<SpawnPoints> spawnPoints;
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
                if (i + 1 > spawnPoints.Count - 1)
                {
                    spawnPoints[i].Spawn(self);
                    return;
                }
                if (spawnPoints[i + 1] != null && !spawnPoints[i + 1].saveWasGetted)
                {
                    spawnPoints[i].Spawn(self);
                    return;
                }
            }
        }

        self.enabled = false;
        self.transform.position = new Vector3(0, 2, -14);
        self.enabled = true;
    }  
}
