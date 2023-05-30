using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDamageField : MonoBehaviour
{
    [SerializeField] private float _damage;
    [SerializeField] private float _pushForce;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        PlayerController player = RuntimeEntities.Instance.Player;
        player.TakeDamage(_damage);
        player.PushRb(new Vector2(collision.transform.position.x, collision.transform.position.y) - collision.GetContact(0).point, _pushForce);
    }
}
