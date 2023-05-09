using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Demon : EnemyController
{
    [SerializeField] private float _distanceToUnchain;
    [SerializeField] private GameObject _tempSoundObject;
    private NavMeshAgent _ai;
    private Rigidbody2D _rb;

    private bool _unchained;

    internal override void Start()
    {
        base.Start();
        _unchained = false;
        _ai = GetComponent<NavMeshAgent>();
        _rb = GetComponent<Rigidbody2D>();
        _ai.updateUpAxis = false;
        _ai.updateRotation = false;
    }

    void Update()
    {
        if (!_unchained)
        {
            if ((RuntimeEntities.Instance.Player.transform.position - transform.position).magnitude <= _distanceToUnchain)
            {
                _unchained = true;
            }
        } else
        {
            if (!_isDead)
            {
                SetNavAgentTarget();
            }
        }
    }

    internal override void OnCollisionEnter2D(Collision2D collision)
    {
        base.OnCollisionEnter2D(collision);
        if (collision.gameObject.layer != LayerMask.NameToLayer("Player"))
        {
            if (!_isDead)
            {
                SoundSystem.Instance.PlayRandomEffect("DemonDamage");
                StartCoroutine("OnImpact");
            }
        }
    }

    internal override void Die()
    {
        base.Die();
        SoundSystem.Instance.PlayRandomEffect("DemonDeath");
        _ai.isStopped = true;
        _rb.gravityScale = 1;
        _rb.freezeRotation = false;
    }

    private IEnumerator OnImpact()
    {
        _ai.isStopped = true;
        yield return new WaitForSeconds(0.25f);
        _ai.isStopped = false;
    }

    private void SetNavAgentTarget()
    {
        _ai.SetDestination(RuntimeEntities.Instance.Player.transform.position);
    }
}
