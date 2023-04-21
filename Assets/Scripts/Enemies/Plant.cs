using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plant : EnemyController
{
    [SerializeField] private Collider2D _trigger;
    [SerializeField] private GameObject _projectilePrefab;
    [SerializeField] private float _projectileSpeed;
    [SerializeField] private float _secondsBetweenShots;
    [SerializeField] private Transform _spawnPointLeft;
    [SerializeField] private Transform _spawnPointRight;
    [SerializeField] private GameObject _tempSoundObject;

    private LocalSoundController _controller;
    private bool _dead;

    internal override void Start()
    {
        base.Start();
        _dead = false;
        _controller = GetComponent<LocalSoundController>();
    }


    public IEnumerator Shoot()
    {
        while (gameObject && !_dead)
        {
            yield return new WaitForSeconds(_secondsBetweenShots);
            _sprite.Animator.Play("Shoot");
            yield return new WaitForSeconds(0.1f);
            SpawnProjectile();
        }
    }

    
    /*
    internal override void OnCollisionEnter2D(Collision2D collision)
    {
        base.OnCollisionEnter2D(collision);
        if (collision.gameObject.layer != LayerMask.NameToLayer("Player"))
        {
            if (!_isDead)
            {
                _controller.PlayRandomClip("Sound/Plant/Damage");
            }
        }
    }
    */

    internal override void Die()
    {
        base.Die();
        _dead = true;
        GameObject _so = Instantiate(_tempSoundObject);
        _so.transform.position = transform.position;
        _so.GetComponent<TemporarySoundObject>().PlayRandomClip("Sound/Plant/Death");
    }

    public void SpawnProjectile()
    {
        GameObject _proj = Instantiate(_projectilePrefab);
        _controller.PlayRandomClip("Sound/Plant/Attack");
        if (!_sprite.Sprite.flipX)
        {
            _proj.transform.position = _spawnPointLeft.position;
            _proj.GetComponent<PlantProjectile>().Speed = -_projectileSpeed;
        } else
        {
            _proj.transform.position = _spawnPointRight.position;
            _proj.GetComponent<PlantProjectile>().Speed = _projectileSpeed;
            _proj.GetComponent<PlantProjectile>().FlipX();
        }
    }
}
