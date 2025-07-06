using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class Settings : MonoBehaviour
{
    [SerializeField] private float sens = 1;
    [SerializeField] private Slider slider;
    [SerializeField] private CameraController cam;
    [Header("Звуки")]
    [SerializeField] private bool soundOn = true;
    [SerializeField] private Image btn;
    [SerializeField] private Sprite btnActive; 
    [SerializeField] private Sprite btnInactive;
    [SerializeField] private AudioMixer mixer;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    private void Update()
    {
        cam.sensitivity = sens;
    }

    public void SliderChange()
    {
        sens = slider.value * 2;
    }

    public void SoundChangerBtn()
    {
        if (soundOn)
        {
            soundOn = false;
            btn.sprite = btnInactive;
            mixer.SetFloat("volume", -80f);
        }
        else
        {
            soundOn = true;
            btn.sprite = btnActive;
            mixer.SetFloat("volume", 0f);
        }
    }
}
