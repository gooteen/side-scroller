using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundManager : MonoBehaviour
{
    [SerializeField] private AudioClip _cenaSound;
    [SerializeField] private Slider _soundSlider;
    [SerializeField] private Slider _musicSlider;
    [SerializeField] private PlayerSettings _settings;
    private AudioSource _source;

    void Start()
    {
        _source = GetComponent<AudioSource>();
        _cenaSound = Resources.Load("Sound/cena") as AudioClip;
    }

    public void PlaySound(string clip)
    {
        if (clip == "cena")
        {
            Debug.Log("player");
            _source.PlayOneShot(_cenaSound);
        }
    }

    public void SaveSoundLevelSettings()
    {
        _settings._soundLevelSetting = _soundSlider.value;
        _source.volume = _settings._soundLevelSetting;
    }

    public void SaveMusicLevelSettings()
    {
        _settings._musicLevelSetting = _musicSlider.value;
        _source.volume = _settings._musicLevelSetting;
    }
}
