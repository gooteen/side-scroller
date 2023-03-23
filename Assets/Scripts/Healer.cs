using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Healer : PoppedItem
{
    [SerializeField] private float _healthToRestore;
    private void Update()
    {
        if ((RuntimeEntities.Instance.Player.transform.position - transform.position).magnitude <= _distanceToPlayer)
        {
            if (_absorbable)
            {
                RuntimeEntities.Instance.Player.Heal(_healthToRestore);
                Destroy(gameObject);
            }
        }
    }
}

