using UnityEngine;
using UnityEngine.UI;

public class Skins : MonoBehaviour
{
    [SerializeField] private GameObject selfTick;
    [SerializeField] private GameObject[] otherTicks;

    public bool isBought;
    [SerializeField] private bool getForAd;
    [SerializeField] private int numOfAd;
    [SerializeField] private bool forRealMoney;
    [SerializeField] private int price;

    public void InvokeMe()
    {
        EquipOrBuySkin(getForAd, numOfAd, forRealMoney, price, isBought);
    }
    private void EquipOrBuySkin(bool getForAd = false, int numOfAd = 0, bool forRealMoney = false, int price = 0, bool isBought = false)
    {
        if (isBought)
        {
            EquipSkin();
            return;
        }
        else if (getForAd)
        {
            EquipSkin();
            return;
        }
        else if (forRealMoney)
        {
            EquipSkin();
            return;
        }
    }

    private void EquipSkin()
    {
        selfTick.SetActive(true);
        foreach (var item in otherTicks)
        {
            item.SetActive(false);
        }
    }
}
