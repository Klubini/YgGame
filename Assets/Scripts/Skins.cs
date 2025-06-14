using UnityEngine;

public class Skins : MonoBehaviour
{
    public void EquipOrBuySkin(bool getForAd = false, int numOfAd = 0, bool forRealMoney = false, int price = 0, bool isBought = false)
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
        
    }
}
