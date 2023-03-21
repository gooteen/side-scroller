using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    [SerializeField] private float _fadeRate;
    [SerializeField] private EnemySpriteController _sprite;
    [SerializeField] private float _totalEnemyHealth;

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
        StartCoroutine("Fade");
    }

    private IEnumerator Fade()
    {
        while (_sprite.Sprite.color.a >= 0.01)
        {
            Debug.Log("DA");
            _sprite.Sprite.color = new Color(_sprite.Sprite.color.r, _sprite.Sprite.color.g, _sprite.Sprite.color.b, _sprite.Sprite.color.a - _fadeRate * Time.deltaTime);
            yield return new WaitForEndOfFrame();
        }
        Destroy(gameObject);
    }
}
