using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private Transform _arm;
    [SerializeField] private Camera _cam;

    [SerializeField] private SpriteController _spriteController;
    [SerializeField] private PlayerSettings _settings;

    [SerializeField] private float _weaponRotationLimitAngle;

    [SerializeField] private bool _isFacingRight;

    private void Start()
    {
        if (_arm.localEulerAngles.z < 90 || (_arm.localEulerAngles.z > 270 && _arm.localEulerAngles.z < 360))
        {
            _isFacingRight = true;
        }
        else
        {
            _isFacingRight = false;
            _spriteController.FlipWeaponSprite();
        }
    }

    void Update()
    {
        Aim();
    }

    private void Aim()
    {
        if (InputProcessor.Instance.GetMouseDelta().magnitude != 0)
        {
            _arm.localRotation = Quaternion.AngleAxis(GetRotationAngle(), Vector3.forward);
            SetDirectionOfAim();
            Debug.Log("EULER: " + _arm.localEulerAngles.z);
            Debug.Log("Mouse Pos: " + InputProcessor.Instance.GetMousePosition());
        }
    }

    private void SetDirectionOfAim()
    {
        if (InputProcessor.Instance.GetMousePosition().x < Screen.width / 2)
        {
            if (_isFacingRight)
            {
                ChangeSide();
            }

            if (_arm.eulerAngles.z < 90 + _weaponRotationLimitAngle && _arm.localRotation.z > 0)
            {
                _arm.localRotation = Quaternion.Euler(_arm.localEulerAngles.x, _arm.localEulerAngles.y, 90 + _weaponRotationLimitAngle);
            }

            if (_arm.eulerAngles.z > 270 - _weaponRotationLimitAngle && _arm.localRotation.z < 0)
            {
                Debug.Log("Hey2: " + _arm.eulerAngles.z + " > " + _weaponRotationLimitAngle + "&&" + _arm.rotation.z + " > 0");
                _arm.localRotation = Quaternion.Euler(_arm.localEulerAngles.x, _arm.localEulerAngles.y, 270 - _weaponRotationLimitAngle);
            }
        }

        else if (InputProcessor.Instance.GetMousePosition().x > Screen.width / 2)
        {
            if (!_isFacingRight)
            {
                ChangeSide();
            }

            if (_arm.eulerAngles.z < 360 - _weaponRotationLimitAngle && _arm.localRotation.z < 0)
            {
                _arm.localRotation = Quaternion.Euler(_arm.localEulerAngles.x, _arm.localEulerAngles.y, -_weaponRotationLimitAngle);
            }

            if (_arm.eulerAngles.z > _weaponRotationLimitAngle && _arm.localRotation.z > 0)
            {
                Debug.Log("Hey2: " + _arm.eulerAngles.z + " > " + _weaponRotationLimitAngle + "&&" + _arm.rotation.z + " > 0");
                _arm.localRotation = Quaternion.Euler(_arm.localEulerAngles.x, _arm.localEulerAngles.y, _weaponRotationLimitAngle);
            }
        }
    }

    private void ChangeSide()
    {
        _spriteController.FlipWeaponSprite();
        _spriteController.FlipPlayerSprite();
        _isFacingRight = !_isFacingRight;
    }

    private float GetRotationAngle()
    {
        Vector2 _dir = InputProcessor.Instance.GetMousePosition() - new Vector2(_cam.WorldToScreenPoint(_arm.position).x, _cam.WorldToScreenPoint(_arm.position).y);
        float _angle = Mathf.Atan2(_dir.y, _dir.x) * Mathf.Rad2Deg;
        Debug.Log("angle: " + _angle);
        return _angle;
    }
}
