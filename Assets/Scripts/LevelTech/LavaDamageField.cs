using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LavaDamageField : MonoBehaviour
{
    [SerializeField] private float _damage;
    [SerializeField] private float _secondsBetweenBurns;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Couroutine started " + collision.gameObject);
        RuntimeEntities.Instance.Player.InLava = true;

        StartCoroutine("InflictLavaDamage");
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        StopCoroutine("InflictLavaDamage");
        RuntimeEntities.Instance.Player.InLava = false;
    }

    private IEnumerator InflictLavaDamage()
    {
        while(RuntimeEntities.Instance.Player.gameObject)
        {
            RuntimeEntities.Instance.Player.TakeDamage(_damage);
            RuntimeEntities.Instance.Player.Sound.PlayLavaDamageSound();
            yield return new WaitForSeconds(_secondsBetweenBurns);
        }
    }
}
