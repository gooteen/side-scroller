using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Healer : PoppedItem
{
    [SerializeField] private float _healthToRestore;

    internal override void OnTriggerStay2D(Collider2D collision)
    {
        base.OnTriggerStay2D(collision);
        if (_absorbable)
        {
            RuntimeEntities.Instance.Player.Heal(_healthToRestore);
        }
    }
}

