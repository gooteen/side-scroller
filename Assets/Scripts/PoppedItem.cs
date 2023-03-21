using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoppedItem : MonoBehaviour
{
    [SerializeField] private float _delayValue;
    [SerializeField] private float _pullForce;
    private Rigidbody2D _rb;
    private bool _absorbable;
    private bool _pullTowardsPlayer;

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
            _rb.AddForce((RuntimeEntities.Instance.Player.transform.position - transform.position).normalized * _pullForce, ForceMode2D.Force);
        }
    }

    private IEnumerator Delay()
    {
        yield return new WaitForSeconds(_delayValue);
        _absorbable = true;
        _pullTowardsPlayer = true;
        _rb.gravityScale = 0;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        RuntimeEntities.Instance.Player.AddPoint();
        Destroy(gameObject);
    }
}
