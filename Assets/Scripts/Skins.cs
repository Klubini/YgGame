using UnityEngine;
using UnityEngine.UI;
using YG;

public class Skins : MonoBehaviour
{
    [SerializeField] private GameObject skinCatalog;
    [SerializeField] private Renderer[] playerMat;
    [SerializeField] private Material newMaterial;
    [SerializeField] private GameObject selfTick;
    [SerializeField] private GameObject[] allTicks;

    public bool isBought;
    [SerializeField] private int id;
    [SerializeField] private string nameForPay;
    [SerializeField] private GameObject advIcon;
    [SerializeField] private GameObject buyUI;
    [SerializeField] private GameObject preview2;
    [SerializeField] private Button selfBTN;
    [SerializeField] private bool getForAd;
    [SerializeField] private int numOfAd;
    [SerializeField] private bool forRealMoney;
    [SerializeField] private int price;

    private void OnEnable()
    {
        YG2.onPurchaseSuccess += SuccessPurchased;
        YG2.onPurchaseFailed += FailedPurchased;
    }

    private void OnDisable()
    {
        YG2.onPurchaseSuccess -= SuccessPurchased;
        YG2.onPurchaseFailed -= FailedPurchased;
    }

    private void Start()
    {
        if (YG2.saves.skinSelectedID == id)
        {
            EquipSkin();
        }

        if (YG2.saves.unlockedSkins[id])
        {
            isBought = true;
            if (advIcon != null)
            {
                advIcon.SetActive(false);
            }
            if (buyUI != null)
            {
                buyUI.SetActive(false);
                preview2.SetActive(true);
            }
            if (selfBTN != null)
            {
                selfBTN.interactable = true;
            }
        }
    }
    public void InvokeMe()
    {
        if (isBought)
        {
            EquipSkin();
            return;
        }
        else if (getForAd)
        {
            if (YG2.saves.unlockedSkins[id])
            {
                EquipSkin();
            }
            else
            {
                YG2.RewardedAdvShow($"skin{id}", () =>
                {
                    YG2.saves.unlockedSkins[id] = true;
                    isBought = true;
                    advIcon.SetActive(false);
                    EquipSkin();
                });
            }
            return;
        }
        else if (forRealMoney)
        {
            Debug.Log("Чел на кнопку купить не попал...");
        }
    }

    private void EquipSkin()
    {
        foreach (var item in allTicks)
        {
            item.SetActive(false);
        }
        selfTick.SetActive(true);

        for (int i = 0; i < playerMat.Length; i++)
        {
            playerMat[i].material = newMaterial;
        }

        YG2.saves.skinSelectedID = id;
        YG2.SaveProgress();
    }


    // Платные покупки
    private void SuccessPurchased(string id)
    {
        if (id == "gigachad")
        {
            if (this.nameForPay == id)
            {
                Debug.Log("Gigachad was bought");
                YG2.saves.unlockedSkins[this.id] = true;
                isBought = true;
                buyUI.SetActive(false);
                preview2.SetActive(true);
                selfBTN.interactable = true;
                EquipSkin();
            }
        }
        else if (id == "notch")
        {
            if (this.nameForPay == id)
            {
                Debug.Log("Notch was bought");
                YG2.saves.unlockedSkins[this.id] = true;
                isBought = true;
                buyUI.SetActive(false);
                preview2.SetActive(true);
                selfBTN.interactable = true;
                EquipSkin();
            }
        }
    }

    private void FailedPurchased(string id)
    {
        Debug.Log("Покупка не была совершена...");
    }
}
