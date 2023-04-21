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
        Debug.Log("sdsdsds");
        yield return new WaitForSeconds(_secondsToDisappear);
        Debug.Log("sdsdsds2");
        Destroy(gameObject);
    }
}
