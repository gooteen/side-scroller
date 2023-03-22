using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDamageField : MonoBehaviour
{
    [SerializeField] private float _damage;
    [SerializeField] private float _pushForce;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        PlayerController _player = RuntimeEntities.Instance.Player;
        _player.TakeDamage(_damage);
        _player.Sprite.PlayDamageImpactAnimation();
        _player.PushRb(new Vector2(collision.transform.position.x, collision.transform.position.y) - collision.GetContact(0).point, _pushForce);
    }
}
