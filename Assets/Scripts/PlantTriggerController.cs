using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlantTriggerController : MonoBehaviour
{
    [SerializeField] private Plant _plant;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        _plant.StartCoroutine("Shoot");
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        _plant.StopCoroutine("Shoot");
    }
}
