using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DuckSoundController : MonoBehaviour
{

    [SerializeField] private AudioClip[] _stepClips;
    [SerializeField] private AudioClip[] _shotClips;
    [SerializeField] private AudioClip _jumpClip;
    private AudioSource _source;

    private void Awake()
    {
        _source = GetComponent<AudioSource>();
        _stepClips = Resources.LoadAll<AudioClip>("Sound/Steps");
        _shotClips = Resources.LoadAll<AudioClip>("Sound/Shots");
        _jumpClip = Resources.Load<AudioClip>("Sound/jump");
    }

    public void PlayRandomStepClip()
    {
        _source.PlayOneShot(_stepClips[Random.Range(0, _stepClips.Length)]);
    }

    public void PlayJumpClip()
    {
        _source.PlayOneShot(_jumpClip);
    }

    public void PlayRandomShotClip()
    {
        _source.PlayOneShot(_shotClips[Random.Range(0, _shotClips.Length)]);
    }
}
