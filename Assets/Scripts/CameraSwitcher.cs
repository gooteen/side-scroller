using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraSwitcher : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera _camToBe;
    private bool _inTheZone;

    private void Start()
    {
        _inTheZone = false;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (!_inTheZone)
        {
            RuntimeEntities.Instance.UpdateCurrentCamera(_camToBe);
            _inTheZone = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        _inTheZone = false;
    }
}
