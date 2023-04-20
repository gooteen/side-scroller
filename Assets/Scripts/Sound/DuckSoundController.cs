using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DuckSoundController : MonoBehaviour
{

    [SerializeField] private AudioClip[] _stepClips;
    [SerializeField] private AudioClip[] _shotClips;
    [SerializeField] private AudioClip[] _damageClips;
    [SerializeField] private AudioClip[] _moneyClips;
    [SerializeField] private AudioClip _deathClip;
    [SerializeField] private AudioClip _jumpClip;
    [SerializeField] private AudioClip _lavaDamageClip;
    [SerializeField] private PlayerSettings _settings;
    private AudioSource _source;

    private void Awake()
    {
        _source = GetComponent<AudioSource>();

        _stepClips = Resources.LoadAll<AudioClip>("Sound/Steps");
        _shotClips = Resources.LoadAll<AudioClip>("Sound/Shots");
        _damageClips = Resources.LoadAll<AudioClip>("Sound/Damage");
        _moneyClips = Resources.LoadAll<AudioClip>("Sound/Money");

        _jumpClip = Resources.Load<AudioClip>("Sound/jump");
        _deathClip = Resources.Load<AudioClip>("Sound/death");
        _lavaDamageClip = Resources.Load<AudioClip>("Sound/Lava/lavaDamage");

        _source.volume = _source.volume * _settings._soundLevelSetting;

    }

    private void Update()
    {
    }

    public void PlayRandomStepClip()
    {
        if (!RuntimeEntities.Instance.Player.InLava)
        {
            _source.PlayOneShot(_stepClips[Random.Range(0, _stepClips.Length)]);
        }
    }

    public void PlayRandomMoneyClip()
    {
        if (!RuntimeEntities.Instance.Player.InLava)
        {
            _source.PlayOneShot(_moneyClips[Random.Range(0, _moneyClips.Length)]);
        }
    }

    public void PlayDeathClip()
    {
        _source.PlayOneShot(_deathClip);
    }

    public void PlayRandomDamageClip()
    {
        _source.PlayOneShot(_damageClips[Random.Range(0, _damageClips.Length)]);
    }

    public void PlayJumpClip()
    {
        _source.PlayOneShot(_jumpClip);
    }

    public void PlayRandomShotClip()
    {
        _source.PlayOneShot(_shotClips[Random.Range(0, _shotClips.Length)]);
    }

    public void PlayLavaDamageSound()
    {
        _source.PlayOneShot(_lavaDamageClip);
    }
}
