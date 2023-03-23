using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    [SerializeField] private float _fadeRate;
    [SerializeField] private EnemySpriteController _sprite;
    [SerializeField] private float _totalEnemyHealth;
    [SerializeField] private GameObject _coinPrefab;
    [SerializeField] private GameObject _healerPrefab;
    [SerializeField] private int _numberOfItemsToSprinkle;
    [SerializeField] private float _coinPushForce;
    [SerializeField] private float _secondsBetweenCoins = 0.1f;
    [SerializeField] private GameObject _damageField;
    [SerializeField] private int _healerDropProbability;

    private float _currentEnemyHealth;
    private NavMeshAgent _ai;
    private Rigidbody2D _rb;

    private bool _isDead;

    void Start()
    {
        _isDead = false;
        _currentEnemyHealth = _totalEnemyHealth;
        _ai = GetComponent<NavMeshAgent>();
        _rb = GetComponent<Rigidbody2D>();
        _ai.updateUpAxis = false;
        _ai.updateRotation = false;
    }

    void Update()
    {
        if (!_isDead)
        {
            SetNavAgentTarget();
        }
    }

    private void SetNavAgentTarget()
    {
        _ai.SetDestination(RuntimeEntities.Instance.Player.transform.position);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer != LayerMask.NameToLayer("Player"))
        {
            if (!_isDead)
            {
                TakeDamage();
                if (!_isDead)
                {
                    StartCoroutine("OnImpact");
                }
                _sprite.Animator.Play("Impact");
            }
        }
    }

    private IEnumerator OnImpact()
    {
        _ai.isStopped = true;
        yield return new WaitForSeconds(0.25f);
        _ai.isStopped = false;
    }

    private void TakeDamage()
    {
        _currentEnemyHealth -= RuntimeEntities.Instance.Settings._bulletDamage;
        _sprite.Animator.SetFloat("Health", _currentEnemyHealth);
        if (_currentEnemyHealth <= 0)
        {
            Die();
            _isDead = true;
        }
    }

    private void Die()
    {
        StopAllCoroutines();
        _ai.isStopped = true;
        _rb.gravityScale = 1;
        _rb.freezeRotation = false;
        transform.rotation = new Quaternion(0,0,0,0);
        _damageField.SetActive(false);
        StartCoroutine("Fade");
    }

    private IEnumerator Fade()
    {
        StartCoroutine("SprinkleCoins");
        while (_sprite.Sprite.color.a >= 0.01)
        {
            Debug.Log("DA");
            _sprite.Sprite.color = new Color(_sprite.Sprite.color.r, _sprite.Sprite.color.g, _sprite.Sprite.color.b, _sprite.Sprite.color.a - _fadeRate * Time.deltaTime);
            yield return new WaitForEndOfFrame();
        }
    }

    private IEnumerator SprinkleCoins()
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

    private bool DropHealer()
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
