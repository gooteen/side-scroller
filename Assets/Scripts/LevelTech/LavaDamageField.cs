using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LavaDamageField : MonoBehaviour
{
    [SerializeField] private float _damage;
    [SerializeField] private float _secondsBetweenBurns;

    [SerializeField] private float _maxSoundDistanceToPlayer;
    [SerializeField] private float _minSoundDistanceToPlayer;
    [SerializeField] private PlayerSettings _settings;
    private AudioSource _audio;

    void Start()
    {
        _audio = GetComponent<AudioSource>();
        SoundSystem.Instance.PlayEffect("lava", _audio);
    }

    void Update()
    {
        float _distance = (RuntimeEntities.Instance.Player.transform.position - transform.position).magnitude;
        if (_distance < _maxSoundDistanceToPlayer)
        {
            Debug.Log("Lava: 0");
            _audio.volume = 1 * _settings._soundLevelSetting;
        } else if (_distance > _minSoundDistanceToPlayer)
        {
            _audio.volume = 0;
            Debug.Log("Lava: 1");
        }
        else if (_distance > _maxSoundDistanceToPlayer && _distance < _minSoundDistanceToPlayer)
        {
            _audio.volume = (1 - (_distance / (_minSoundDistanceToPlayer - _maxSoundDistanceToPlayer)) * _settings._soundLevelSetting);
            Debug.Log("Lava: changing");
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Couroutine started " + collision.gameObject);
        RuntimeEntities.Instance.Player.InLava = true;

        StartCoroutine("InflictLavaDamage");
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        StopCoroutine("InflictLavaDamage");
        RuntimeEntities.Instance.Player.InLava = false;
    }

    private IEnumerator InflictLavaDamage()
    {
        while(RuntimeEntities.Instance.Player.gameObject)
        {
            RuntimeEntities.Instance.Player.TakeDamage(_damage);
            SoundSystem.Instance.PlayEffect("lavaDamage");
            yield return new WaitForSeconds(_secondsBetweenBurns);
        }
    }
}
