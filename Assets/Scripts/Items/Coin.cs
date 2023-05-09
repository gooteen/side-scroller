using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : PoppedItem
{
    internal override void OnTriggerStay2D(Collider2D collision)
    {
        base.OnTriggerStay2D(collision);
        if (_absorbable)
        {
            RuntimeEntities.Instance.Player.AddPoint();
            SoundSystem.Instance.PlayRandomEffect("Money");
        }
    }
}
