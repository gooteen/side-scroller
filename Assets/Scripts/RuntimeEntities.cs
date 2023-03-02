using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RuntimeEntities : MonoBehaviour
{
    [SerializeField] private Camera _cam;

    public static RuntimeEntities Instance
    {
        get; private set;
    }

    public Camera Camera { get { return _cam; } }

    void Awake()
    {
        Instance = this;
    }
}