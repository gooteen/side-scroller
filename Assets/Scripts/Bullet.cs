using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private float _damage;
    [SerializeField] private float _speed;
    [SerializeField] private float _lifespan;

    private Rigidbody2D _rb;
    private float _releaseTime;
    
    void Start()
    {
        Debug.Log("Born");
        _rb = GetComponent<Rigidbody2D>();

        
        _releaseTime = Time.time;
    }

    void FixedUpdate()
    {
        if (Time.time - _releaseTime >= _lifespan)
        {
            Destroy(gameObject);
        } else
        {
            _rb.velocity = transform.right * _speed;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Destroy(gameObject);
    }
}
