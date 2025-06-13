using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ProgressScript : MonoBehaviour
{
    public static int progressNumBaseLVL = 0;
    [SerializeField] private Image progressBar;
    [SerializeField] private Button skipLvlBtn;
    [SerializeField] private TextMeshProUGUI lvlInText;

    private void Update()
    {
        progressBar.fillAmount = Mathf.Lerp(progressBar.fillAmount, progressNumBaseLVL / 50f, Time.deltaTime);
        lvlInText.text = $"{progressNumBaseLVL}/50";
        if (progressNumBaseLVL == 0 || progressNumBaseLVL >= 49)
        {
            skipLvlBtn.gameObject.SetActive(false);
        }
        else
        {
            skipLvlBtn.gameObject.SetActive(true);
        }
    }
}
