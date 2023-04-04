using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoppedItem : MonoBehaviour
{
    [SerializeField] internal float _delayValue;
    [SerializeField] internal float _pullForce;
    [SerializeField] internal float _distanceToPlayer;

    internal Rigidbody2D _rb;
    internal bool _absorbable;
    internal bool _pullTowardsPlayer;

    void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _absorbable = false;
        _pullTowardsPlayer = false;
        StartCoroutine("Delay");
    }

    void FixedUpdate()
    {
        if (_pullTowardsPlayer)
        {
            _rb.velocity = (RuntimeEntities.Instance.Player.transform.position - transform.position).normalized * _pullForce;
        }
    }

    private IEnumerator Delay()
    {
        yield return new WaitForSeconds(_delayValue);
        _absorbable = true;
        _pullTowardsPlayer = true;
        _rb.gravityScale = 0;
    }

    internal virtual void OnTriggerStay2D(Collider2D collision)
    {
        if (_absorbable)
        {
            Destroy(gameObject);
        }
    }
}
