using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : PoppedItem
{
    private void Update()
    {
        if ((RuntimeEntities.Instance.Player.transform.position - transform.position).magnitude <= _distanceToPlayer)
        {
            if (_absorbable)
            {
                RuntimeEntities.Instance.Player.AddPoint();
                Destroy(gameObject);
            }
        }
    }
}
