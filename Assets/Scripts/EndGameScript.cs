using UnityEngine;
using YG;

public class EndGameScript : MonoBehaviour
{
    [SerializeField] private CharacterController self;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            self.enabled = false;
            self.transform.position = new Vector3(0, 2, -14);
            self.enabled = true;
        }     
    }
}
