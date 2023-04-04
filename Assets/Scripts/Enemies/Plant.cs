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

    public IEnumerator Shoot()
    {
        while (gameObject)
        {
            yield return new WaitForSeconds(_secondsBetweenShots);
            _sprite.Animator.Play("Shoot");
            yield return new WaitForSeconds(0.1f);
            SpawnProjectile();
        }
    }

    public void SpawnProjectile()
    {
        GameObject _proj = Instantiate(_projectilePrefab);
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
