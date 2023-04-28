using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TemporarySoundObject : LocalSoundController
{
    [SerializeField] private float _secondsToDisappear;

    void Start()
    {
        StartCoroutine("Vanish");
    }

    private IEnumerator Vanish()
    {
        yield return new WaitForSeconds(_secondsToDisappear);
        Destroy(gameObject);
    }
}
