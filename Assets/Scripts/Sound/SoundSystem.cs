using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SoundSystem : MonoBehaviour
{
    [SerializeField] private Dictionary<string, AudioClip> _sounds;
    [SerializeField] private Dictionary<string, AudioClip> _musicClips;
    [SerializeField] private Dictionary<string, AudioClip[]> _soundGroups;
    [SerializeField] private PlayerSettings _settings;
    [SerializeField] private AudioSource _audioSounds;
    [SerializeField] private AudioSource _audioMusic;

    public static SoundSystem Instance { get; private set; }

    void Awake()
    {
        Instance = this;
        UpdateVolume();
        CollectSounds();
    }

    public void UpdateVolume()
    {
        _audioSounds.volume = _settings.soundLevelSetting;
        _audioMusic.volume = _settings.musicLevelSetting;
    }

    void CollectSounds()
    {
        _sounds = new Dictionary<string, AudioClip>();
        _musicClips = new Dictionary<string, AudioClip>();
        _soundGroups = new Dictionary<string, AudioClip[]>();

        foreach (AudioClip clip in Resources.LoadAll<AudioClip>("Sound/Singles"))
        {
            _sounds[clip.name] = clip;
        }

        foreach (AudioClip clip in Resources.LoadAll<AudioClip>("Sound/Music"))
        {
            _musicClips[clip.name] = clip;
        }

        TextAsset manifest = Resources.Load<TextAsset>("Sound/SoundGroups/manifest");

        if (manifest != null)
        {
            foreach (string path in manifest.text.Split('|'))
            _soundGroups[path] = Resources.LoadAll<AudioClip>($"Sound/SoundGroups/{path}");
        }
    }

    public void PlayMusic(string name)
    {
        AudioClip clip;
        if (_musicClips.TryGetValue(name, out clip))
        {
            _audioMusic.Stop();
            _audioMusic.clip = clip;
            _audioMusic.Play();
        }
    }

    public void PlayEffect(string name)
    {
        AudioClip clip;
        if (_sounds.TryGetValue(name, out clip))
            _audioSounds.PlayOneShot(clip);
    }

    public void PlayEffect(string name, AudioSource source)
    {
        AudioClip clip;
        if (_sounds.TryGetValue(name, out clip))
            source.PlayOneShot(clip);
    }


    public void PlayRandomEffect(string name)
    {
        AudioClip[] clips;
        if (_soundGroups.TryGetValue(name, out clips))
            _audioSounds.PlayOneShot(clips[Random.Range(0, clips.Length)]);
    }

    public AudioClip GetMusicClip(string name)
    {
        AudioClip clip;
        _musicClips.TryGetValue(name, out clip);
        return clip;
    }
}
