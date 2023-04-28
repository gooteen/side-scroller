using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocalSoundController : MonoBehaviour
{
    [SerializeField] string _pathDefault;
    [SerializeField] private AudioClip[] _clips;
    [SerializeField] private bool _changeVolumeBasedOnPlayerDistance;
    [SerializeField] private bool _playOnStart;
    [SerializeField] private float _minSoundDistance;
    [SerializeField] private float _maxSoundDistance;
    [SerializeField] private Transform _soundCenter;
    [SerializeField] private PlayerSettings _settings;
    private Transform _currentsoundCenter;
    private AudioSource _source;

    public bool HasConstantVolume { get { return !_changeVolumeBasedOnPlayerDistance; } set { _changeVolumeBasedOnPlayerDistance = !value; } }
    public float Volume { set { _source.volume = value * _settings._soundLevelSetting; } }

    private void Awake()
    {
        _source = GetComponent<AudioSource>();
        if (_soundCenter == null)
        {
            _currentsoundCenter = transform;
        } else
        {
            _currentsoundCenter = _soundCenter;
        }
    }

    void Start()
    {
        if (_playOnStart)
        {
            PlayRandomClip(_pathDefault);
        }
    }

    private void Update()
    {
        if (_changeVolumeBasedOnPlayerDistance)
        {
            float _currentDistance = (_currentsoundCenter.position - RuntimeEntities.Instance.Player.transform.position).magnitude;
            if (_currentDistance >= _minSoundDistance)
            {
                Debug.Log($"Less: {_currentDistance}, {_minSoundDistance}");
                _source.volume = 0;
            } else if (_currentDistance <= _maxSoundDistance)
            {
                _source.volume = 1 * _settings._soundLevelSetting;
            }
            else
            {
                _source.volume = (1 - (_currentDistance / _minSoundDistance)) * _settings._soundLevelSetting;
            }
        }

    }

    public void PlayRandomClip(string _path)
    {
        _clips = Resources.LoadAll<AudioClip>(_path);
        _source.PlayOneShot(_clips[Random.Range(0, _clips.Length)]);
    }
}
