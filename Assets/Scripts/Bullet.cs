using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private float _damage;
    [SerializeField] private float _speed;
    [SerializeField] private float _lifespan;

    [SerializeField] private float _timeBeforeTrail;
    private Rigidbody2D _rb;
    private ParticleSystem _ps;
    private ParticleSystem.EmissionModule _emission;

    private float _trailDelayCounter;
    private float _releaseTime;
    
    void Start()
    {
        _ps = GetComponent<ParticleSystem>();
        _emission = _ps.emission;

        Debug.Log("Born");
        _rb = GetComponent<Rigidbody2D>();

        _trailDelayCounter = Time.time;
        _releaseTime = Time.time;
    }

    private void Update()
    {
        if (_emission.enabled == false && (Time.time - _trailDelayCounter >= _timeBeforeTrail))
        {
            Debug.Log("Change");
            _emission.enabled = true;
        }
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
