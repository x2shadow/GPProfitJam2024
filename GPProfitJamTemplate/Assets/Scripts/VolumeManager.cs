using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VolumeManager : MonoBehaviour
{
    [SerializeField] Slider volumeSlider;
    [SerializeField] float volume = 0.1f;
                     float oldVolume;
    [SerializeField] Image volumeIcon;

    void Start()
    {
        volumeSlider.value = volume;
        oldVolume = volume;
    }

    public void ChangeVolume()
    {
        AudioListener.volume = volumeSlider.value;
        if(volumeSlider.value == 0) volumeIcon.sprite = Resources.Load<Sprite>("Icons/mute-white"); else volumeIcon.sprite = Resources.Load<Sprite>("Icons/medium-volume-white"); 
    }

    public void ToggleMute()
    {
        if(volumeSlider.value != 0)
        {
            oldVolume = volumeSlider.value;
            volumeSlider.value = 0;
            AudioListener.volume = 0;
            volumeIcon.sprite = Resources.Load<Sprite>("Icons/mute-white");
        }
        else
        {
            volumeSlider.value = oldVolume;
            AudioListener.volume = oldVolume;
            volumeIcon.sprite = Resources.Load<Sprite>("Icons/medium-volume-white");
        }
    }
}
