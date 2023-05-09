using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelMusicController : MonoBehaviour
{
    void Start()
    {
        StartCoroutine("LoopMusic");
    }

    private IEnumerator LoopMusic()
    {
        SoundSystem.Instance.PlayMusic("hellStart");
        yield return new WaitForSeconds(SoundSystem.Instance.GetMusicClip("hellStart").length);
        SoundSystem.Instance.PlayMusic("hellCycle");
    }
}
