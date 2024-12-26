using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SoundManager : MonoBehaviour
{
    public const string GameVolumeKey = "GameVolume";
    public const string MusicVolumeKey = "MusicVolume";
    public const string UIVolumeKey = "UIVolume";
    public AudioMixer gameAudioMaster;
    [SerializeField]private Slider gameVolumeSlider;
    public AudioMixer musicAudioMaster;
    [SerializeField]private Slider musicVolumeSlider;
    public AudioMixer uIAudioMaster;
    [SerializeField]private Slider uIVolumeSlider;

    public void SetGameVolume()
    {
        gameAudioMaster.SetFloat("Volume", gameVolumeSlider.value);
        PlayerPrefs.SetFloat(GameVolumeKey, gameVolumeSlider.value);
        PlayerPrefs.Save();
    }
    public void SetMusicVolume()
    {
        musicAudioMaster.SetFloat("Volume", musicVolumeSlider.value);
        PlayerPrefs.SetFloat(MusicVolumeKey, musicVolumeSlider.value);
        PlayerPrefs.Save();
    }
    public void SetUIVolume()
    {
        uIAudioMaster.SetFloat("Volume", uIVolumeSlider.value);
        PlayerPrefs.SetFloat(UIVolumeKey, uIVolumeSlider.value);
        PlayerPrefs.Save();
    }
    private void Start()
    {
        gameVolumeSlider.value=PlayerPrefs.GetFloat(GameVolumeKey, 0);
        musicVolumeSlider.value=PlayerPrefs.GetFloat(MusicVolumeKey, 0);
        uIVolumeSlider.value=PlayerPrefs.GetFloat(UIVolumeKey, 0);
    }
}
