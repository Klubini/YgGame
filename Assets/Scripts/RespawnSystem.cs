using UnityEngine;
using System.Collections.Generic;
using YG;

public class RespawnSystem : MonoBehaviour
{
    [SerializeField] private AudioSource dieSfx;
    [SerializeField] private GameObject dieMenu;
    public List<SpawnPoints> spawnPoints;
    public ThirdPersonController controller;
    private CharacterController self;
    private bool soundCanBe = false;
    private bool died = false;

    private void Awake()
    {
        self = GetComponent<CharacterController>();
    }

    void Start()
    {
        Invoke("BugFixer", 1f);
    }

    private void BugFixer()
    {
        soundCanBe = true;
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
        if (died) { return; }

        died = true;
        controller.live = false;
        dieMenu.SetActive(true);
        if (soundCanBe)
        {
            dieSfx.Play();
        }
    }

    public void RespawnBtnPressed()
    {
        died = false;
        controller.live = true;
        YG2.InterstitialAdvShow();
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

        self.enabled = false;
        self.transform.position = new Vector3(0, 2, -14);
        self.enabled = true;
    }  
}
