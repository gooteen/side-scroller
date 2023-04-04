using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [SerializeField] internal float _fadeRate;
    [SerializeField] internal EnemySpriteController _sprite;
    [SerializeField] internal float _totalEnemyHealth;
    [SerializeField] internal GameObject _coinPrefab;
    [SerializeField] internal GameObject _healerPrefab;
    [SerializeField] internal int _numberOfItemsToSprinkle;
    [SerializeField] internal float _coinPushForce;
    [SerializeField] internal float _secondsBetweenCoins = 0.1f;
    [SerializeField] internal GameObject _damageField;
    [SerializeField] internal int _healerDropProbability;

    internal float _currentEnemyHealth;
    internal bool _isDead;

    internal virtual void Start()
    {
        _isDead = false;
        _currentEnemyHealth = _totalEnemyHealth;
    }

    internal virtual void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer != LayerMask.NameToLayer("Player"))
        {
            if (!_isDead)
            {
                TakeDamage();
                _sprite.Animator.Play("Impact");
            }
        }
    }

    internal void TakeDamage()
    {
        _currentEnemyHealth -= RuntimeEntities.Instance.Settings._bulletDamage;
        _sprite.Animator.SetFloat("Health", _currentEnemyHealth);
        if (_currentEnemyHealth <= 0)
        {
            Die();
            _isDead = true;
        }
    }

    internal virtual void Die()
    {
        StopAllCoroutines();
        transform.rotation = new Quaternion(0,0,0,0);
        _damageField.SetActive(false);
        StartCoroutine("Fade");
    }

    internal IEnumerator Fade()
    {
        StartCoroutine("SprinkleCoins");
        while (_sprite.Sprite.color.a >= 0.01)
        {
            Debug.Log("DA");
            _sprite.Sprite.color = new Color(_sprite.Sprite.color.r, _sprite.Sprite.color.g, _sprite.Sprite.color.b, _sprite.Sprite.color.a - _fadeRate * Time.deltaTime);
            yield return new WaitForEndOfFrame();
        }
    }

    internal IEnumerator SprinkleCoins()
    {
        int _counter = _numberOfItemsToSprinkle;
        while (_counter > 0)
        {
            GameObject _item;
            if (DropHealer())
            {
               _item = Instantiate(_healerPrefab);
            } else
            {
                _item = Instantiate(_coinPrefab);
            }

            _item.transform.position = transform.position;
            _item.GetComponent<Rigidbody2D>().AddForce(new Vector2(Random.Range(-1, 1), Random.Range(0.5f, 1)).normalized * _coinPushForce, ForceMode2D.Impulse);
            _counter -= 1;
            yield return new WaitForSeconds(_secondsBetweenCoins);
        }
        Destroy(gameObject);
    }

    internal bool DropHealer()
    {
        int _param = Random.Range(0, 100);
        if (_param <= _healerDropProbability)
        {
            return true;
        } else
        {
            return false;
        }
    }
}
