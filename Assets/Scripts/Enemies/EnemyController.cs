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
        _currentEnemyHealth -= RuntimeEntities.Instance.Settings.bulletDamage;
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
            _sprite.Sprite.color = new Color(_sprite.Sprite.color.r, _sprite.Sprite.color.g, _sprite.Sprite.color.b, _sprite.Sprite.color.a - _fadeRate * Time.deltaTime);
            yield return new WaitForEndOfFrame();
        }
        Destroy(gameObject);
    }

    internal IEnumerator SprinkleCoins()
    {
        int counter = _numberOfItemsToSprinkle;
        while (counter > 0)
        {
            GameObject item;
            if (DropHealer())
            {
                item = Instantiate(_healerPrefab);
            } else
            {
                item = Instantiate(_coinPrefab);
            }

            item.transform.position = transform.position;
            item.GetComponent<Rigidbody2D>().AddForce(new Vector2(Random.Range(-1, 1), Random.Range(0.5f, 1)).normalized * _coinPushForce, ForceMode2D.Impulse);
            counter -= 1;
            yield return new WaitForSeconds(_secondsBetweenCoins);
        }
    }

    internal bool DropHealer()
    {
        int param = Random.Range(0, 100);
        if (param <= _healerDropProbability)
        {
            return true;
        } else
        {
            return false;
        }
    }
}
