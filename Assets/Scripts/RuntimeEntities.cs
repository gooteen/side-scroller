using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RuntimeEntities : MonoBehaviour
{
    [SerializeField] private Camera _cam;
    [SerializeField] private PlayerController _player;
    [SerializeField] private PlayerSettings _settings;

    public static RuntimeEntities Instance
    {
        get; private set;
    }

    public PlayerController Player { get { return _player; } }

    public PlayerSettings Settings { get { return _settings; } }

    public Camera Camera { get { return _cam; } }

    void Awake()
    {
        Instance = this;
    }
}