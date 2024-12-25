using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SoundManager : MonoBehaviour
{
    public AudioMixer gameAudioMaster;
    [SerializeField]private Slider gameVolumeSlider;
    public AudioMixer musicAudioMaster;
    [SerializeField]private Slider musicVolumeSlider;
    public AudioMixer uIAudioMaster;
    [SerializeField]private Slider uIVolumeSlider;
    public void SetGameVolume()
    {
        gameAudioMaster.SetFloat("Volume", gameVolumeSlider.value);
        //gameAudioMaster.SetFloat("Volume", PlayerPrefs.GetFloat("GameVolume"));
        //musicAudioMaster.SetFloat("Volume", PlayerPrefs.GetFloat("MusicVolume"));
        //uIAudioMaster.SetFloat("Volume", PlayerPrefs.GetFloat("UIVolume"));
    }
    public void SetMusicVolume()
    {
        musicAudioMaster.SetFloat("Volume", musicVolumeSlider.value);
    }
    public void SetUIVolume()
    {
        uIAudioMaster.SetFloat("Volume", uIVolumeSlider.value);
    }
}
