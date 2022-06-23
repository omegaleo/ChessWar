using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Slider))]
public class VolumeSlider : MonoBehaviour
{
    public enum VolumeSliderType { MUSIC, SFX }

    private Slider slider;

    [SerializeField] private VolumeSliderType type;
    
    // Start is called before the first frame update
    void Start()
    {
        slider = GetComponent<Slider>();

        switch (type)
        {
            case VolumeSliderType.SFX:
                slider.value = SFXManager.instance.GetVolume();
                break;
            case VolumeSliderType.MUSIC:
                slider.value = MusicManager.instance.GetVolume();
                break;
        }
    }

    public void SetVolume()
    {
        switch (type)
        {
            case VolumeSliderType.SFX:
                if (slider.value == slider.minValue)
                {
                    SFXManager.instance.Mute();
                }
                else
                {
                    SFXManager.instance.SetVolume(slider.value);
                }
                break;
            case VolumeSliderType.MUSIC:
                if (slider.value == slider.minValue)
                {
                    MusicManager.instance.Mute();
                }
                else
                {
                    MusicManager.instance.SetVolume(slider.value);
                }
                break;
        }
    }
}
