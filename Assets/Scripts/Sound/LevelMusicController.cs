using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelMusicController : MonoBehaviour
{
    [SerializeField] private AudioClip _musicStart;
    [SerializeField] private AudioClip _musicCycle;
    [SerializeField] private PlayerSettings _settings;
    private AudioSource _source;

    void Start()
    {
        _source = GetComponent<AudioSource>();
        _source.volume = _settings._musicLevelSetting;
        StartCoroutine("PlayMusic");
    }

    private IEnumerator PlayMusic()
    {
        _source.clip = _musicStart;
        _source.Play();
        yield return new WaitForSeconds(_musicStart.length - 0.4f);
        _source.clip = _musicCycle;
        _source.loop = true;
        _source.Play();
    }
}
