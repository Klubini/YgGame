using TMPro;
using UnityEngine;
using UnityEngine.UI;
using YG;

public class ProgressScript : MonoBehaviour
{
    public static int progressNumBaseLVL = 0;
    [SerializeField] private Image progressBarPanel;
    [SerializeField] private Image progressBar;
    [SerializeField] private Button skipLvlBtn;
    [SerializeField] private TextMeshProUGUI lvlInText;
    [SerializeField] private Sprite proLvlPBP;
    [SerializeField] private Sprite proLvlPB;
    private bool wasChanged = false;

    private void Update()
    {
        if (YG2.saves.isSecondPart)
        {
            lvlInText.text = $"{progressNumBaseLVL}/70";
            progressBar.fillAmount = Mathf.Lerp(progressBar.fillAmount, progressNumBaseLVL / 70f, Time.deltaTime);
            if (!wasChanged)
            {
                wasChanged = true;
                progressBar.sprite = proLvlPB;
                progressBarPanel.sprite = proLvlPBP;
            }
            return;
        }
        progressNumBaseLVL = YG2.saves.currentlvl;
        progressBar.fillAmount = Mathf.Lerp(progressBar.fillAmount, progressNumBaseLVL / 50f, Time.deltaTime);
        lvlInText.text = $"{progressNumBaseLVL}/50";
        if (progressNumBaseLVL == 0 || progressNumBaseLVL == 49 || progressNumBaseLVL == 50)
        {
            skipLvlBtn.gameObject.SetActive(false);
        }
        else
        {
            skipLvlBtn.gameObject.SetActive(true);
        }
        
    }
}
