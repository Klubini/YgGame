using System.Collections.Generic;
using UnityEngine;
using YG;

public class SkipLvl : MonoBehaviour
{
    [SerializeField] private RespawnSystem system;
    [SerializeField] private CharacterController self;

    private List<SpawnPoints> spawnPoints => system.spawnPoints;

    public void InvokeSkipForAdvertisment()
    {
        YG2.RewardedAdvShow("skip", () =>
        {
            for (int i = 0; i < spawnPoints.Count; i++)
            {
                if (spawnPoints[i].saveWasGetted)
                {
                    if (spawnPoints[i + 1] != null && !spawnPoints[i + 1].saveWasGetted)
                    {
                        spawnPoints[i + 1].Spawn(self);
                        return;
                    }
                }
            }
        });
    }
}
