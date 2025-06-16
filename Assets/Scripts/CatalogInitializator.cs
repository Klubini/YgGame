using UnityEngine;
using YG;

public class CatalogInitializator : MonoBehaviour
{
    [SerializeField] private Material[] skins;
    [SerializeField] private Renderer[] renderers;

    void Start()
    {
        for (int i = 0; i < renderers.Length; i++)
        {
            renderers[i].material = skins[YG2.saves.skinSelectedID];
        }
    }
}
