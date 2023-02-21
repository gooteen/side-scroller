using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private Transform _arm;
    [SerializeField] private Camera _cam;

    [SerializeField] private PlayerSettings _settings;

    [SerializeField] private float _weaponRotationLimitAngle;

    void Update()
    {
        if(InputProcessor.Instance.GetMouseDelta().magnitude != 0)
        {
            _arm.Rotate(0, 0, InputProcessor.Instance.GetMouseDelta().normalized.y * _settings._mouseSensitivity);
            Debug.Log("euler " + _arm.eulerAngles.z);
            Debug.Log("rotation " + _arm.rotation.z);

           
            if (_arm.eulerAngles.z < 360 - _weaponRotationLimitAngle && _arm.rotation.z < 0)
            {
            _arm.rotation = Quaternion.Euler(_arm.eulerAngles.x, _arm.eulerAngles.y, -_weaponRotationLimitAngle);
            }

            if (_arm.eulerAngles.z > _weaponRotationLimitAngle && _arm.rotation.z > 0)
            {
            _arm.rotation = Quaternion.Euler(_arm.eulerAngles.x, _arm.eulerAngles.y, _weaponRotationLimitAngle);
            }
        }

        if (InputProcessor.Instance.GetMovementDirection().normalized.x > 0)
        {
            _arm.rotation = new Quaternion(0, 0, _arm.rotation.z, 1);
        } else if (InputProcessor.Instance.GetMovementDirection().normalized.x < 0)
        {
            _arm.rotation = new Quaternion(0, 179, _arm.rotation.z, 1);
        }
    }
}
