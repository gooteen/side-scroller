using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cooler : MonoBehaviour
{
    [SerializeField] private float _heatToTakeAway;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        RuntimeEntities.Instance.Player.Gun.TakeAwayHeat(_heatToTakeAway);
        Destroy(gameObject);
    }
}
