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
        StartCoroutine("InflictLavaDamage");
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        StopCoroutine("InflictLavaDamage");
    }

    private IEnumerator InflictLavaDamage()
    {
        while(RuntimeEntities.Instance.Player.gameObject)
        {
            RuntimeEntities.Instance.Player.TakeDamage(_damage);
            yield return new WaitForSeconds(_secondsBetweenBurns);
        }
    }
}
