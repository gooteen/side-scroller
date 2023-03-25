using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlantProjectile : MonoBehaviour
{
    [SerializeField] private Vector2 _direction;
    [SerializeField] private float _damage;
    [SerializeField] private float _pushForce;
    [SerializeField] private SpriteRenderer _sprite;
    private float _speed;

    public float Speed { set { _speed = value; } }

    void Update()
    {
        transform.position = (new Vector2(transform.position.x + _speed * Time.deltaTime, transform.position.y));
    }

    public void FlipX()
    {
        _sprite.flipX = true;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            PlayerController _player = RuntimeEntities.Instance.Player;
            _player.TakeDamage(_damage);
            _player.Sprite.PlayDamageImpactAnimation();
            _player.PushRb(new Vector2(collision.transform.position.x, collision.transform.position.y) - collision.GetContact(0).point, _pushForce);
        }
        Destroy(gameObject);
    }
}
