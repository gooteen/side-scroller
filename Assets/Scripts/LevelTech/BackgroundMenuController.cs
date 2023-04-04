using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundMenuController : MonoBehaviour
{
    [SerializeField] private Transform _pointStart;
    [SerializeField] private Transform _pointEnd;
    [SerializeField] private float _speed;

    void Start()
    {
        transform.position = new Vector3(_pointStart.position.x, transform.position.y);
    }

    void Update()
    {
        if (transform.position.x <= _pointEnd.position.x)
        {
            transform.position = new Vector3(transform.position.x + _speed * Time.deltaTime, transform.position.y);
        } else
        {
            transform.position = new Vector3(_pointStart.position.x, transform.position.y);
        }
    }
}
