using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private Transform _arm;
    [SerializeField] private Transform _raycastOrigin;

    [SerializeField] private Transform _bulletOriginRight;
    [SerializeField] private Transform _bulletOriginLeft;

    [SerializeField] private GunController _gun;
    [SerializeField] private SpriteController _spriteController;
    [SerializeField] private PlayerSettings _settings;

    [SerializeField] private float _weaponRotationLimitAngle;
    [SerializeField] private float _groundCheckRayLength;

    [SerializeField] private float _timeToCooldown;

    [SerializeField] private bool _isFacingRight;

    [SerializeField] private float _maxHealth;

    [SerializeField] private float _heatUpStepPerFrame = 0.1f;
    [SerializeField] private float _heatDownStepPerFrame = 0.1f;

    [SerializeField] private bool _isShooting;
    private bool _isJumping;
    private bool _isAiming;
    private bool _armVisible;
    
    private float _coolDownStartTime;
    private float _currentHealth;

    private Rigidbody2D _rb;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        _currentHealth = _maxHealth;
        _isShooting = false;
        _armVisible = false;
        _isAiming = false;

        _arm.gameObject.SetActive(false);

        SetArmVisibility();
        
        if (OnTheGround())
        {
            _isJumping = false;
        }
        else
        {
            _isJumping = true;
        }

        
        if (_arm.localEulerAngles.z < 90 || (_arm.localEulerAngles.z > 270 && _arm.localEulerAngles.z < 360))
        {
            _isFacingRight = true;
            _gun.SetBulletOrigin(_bulletOriginRight);
        }
        else
        {
            _isFacingRight = false;
            _gun.SetBulletOrigin(_bulletOriginLeft);
            _spriteController.FlipWeaponSprite();
        }
        
        _spriteController.SetIsJumpingAnimatorParameter(_isJumping);
        _spriteController.SetIsFacingRightAnimatorParameter(_isFacingRight);
    }

    void Update()
    {
        SetArmVisibility();

        if (_armVisible)
        {
            Aim();
            if (!_gun.Overheated)
            {
                if (InputProcessor.Instance.LeftMouseButtonPressed())
                {
                    _gun.StopEmittingSmoke();
                    _isShooting = true;
                    _gun.Shoot();
                    _gun.IncreaseHeat(_heatUpStepPerFrame);

                }
                else
                {
                    if (_isShooting)
                    {
                        _gun.StartEmittingSmoke();
                        _isShooting = false;
                    }
                }
            } else
            {
                _gun.StartEmittingSmoke();
                _isShooting = false;
            }
        } else
        {
            _isShooting = false;
        }

        if (!_isShooting)
        {
            _gun.DecreaseHeat(_heatDownStepPerFrame);
            Debug.Log("DECREASE!!");
        }

        Move();

        if (OnTheGround())
        {
            Jump();
        }
    }

    public void PushRb(Vector2 dir, float force)
    {
        _rb.AddForce(dir.normalized * force, ForceMode2D.Impulse);
    }

    public void TakeDamage(float damage)
    {
        _currentHealth -= damage;
    }

    private void FixedUpdate()
    {
        if (!_gun.Overheated)
        {
            if (InputProcessor.Instance.LeftMouseButtonPressed() && InputProcessor.Instance.RightMouseButtonPressed())
            {

                ApplyRecoilForce();
            }
        }
    }

    private void ApplyRecoilForce()
    {
        float xDir = 0;
        float yDir = 0;

        if (_isFacingRight)
        {
            xDir = -_gun.RecoilForceX;
        } else
        {
            xDir = _gun.RecoilForceX;
        }

        if (RuntimeEntities.Instance.Camera.ScreenToWorldPoint(InputProcessor.Instance.GetMousePosition()).y >= transform.position.y)
        {
            yDir = -_gun.RecoilForceY;
        } else
        {
            yDir = _gun.RecoilForceY;
        }

        _rb.AddForce(transform.right * xDir, ForceMode2D.Force);
        _rb.AddForce(transform.up * yDir, ForceMode2D.Force);
    }

    private void SetArmVisibility()
    {
        if (!_armVisible && InputProcessor.Instance.RightMouseButtonPressed())
        {
            if (!_isAiming)
            {
                _isAiming = true;
                _gun.ResetReleaseTime();
            } 
            _armVisible = true;
            _arm.gameObject.SetActive(true);
        } else if (_armVisible && !InputProcessor.Instance.RightMouseButtonPressed())
        {
            if (_isAiming)
            {
                _gun.StopEmittingSmoke();
                _isAiming = false;
            }
            _armVisible = false;
            _arm.gameObject.SetActive(false);
        }
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

    private void Jump()
    {
        if (InputProcessor.Instance.JumpButtonPressed())
        {
            _rb.AddForce(Vector2.up * _settings._jumpImpulse, ForceMode2D.Impulse);
            _spriteController.SetStretchAnimatorParameter();
        }
    }

    private void Move()
    {
        Vector2 _direction = InputProcessor.Instance.GetMovementDirection().normalized;
        
        if (_direction.magnitude != 0)
        {
            if (OnTheGround())
            {
                _rb.velocity = new Vector2(_direction.x * _settings._playerMovementSpeedGround, _rb.velocity.y);
            } else
            {
                if (!_isAiming)
                {
                    _rb.velocity = new Vector2(_direction.x * _settings._playerMovementSpeedGround, _rb.velocity.y);
                }
            }
            if (!_armVisible)
            {
                if (_direction.x > 0 && !_isFacingRight)
                {
                    ChangeSide();
                    _gun.SetBulletOrigin(_bulletOriginRight);
                }
                else if (_direction.x < 0 && _isFacingRight)
                {
                    ChangeSide();
                    _gun.SetBulletOrigin(_bulletOriginLeft);
                }
            }
        } else
        {
            if (OnTheGround())
            {
                _rb.velocity = new Vector2(0, _rb.velocity.y);
            } 
        }
        _spriteController.SetDirectionAnimatorParameter(_direction);
        _spriteController.SetYVelocityAnimatorParameter(_rb.velocity.y);
        //_rb.AddForce(_direction * _walkSpeed, ForceMode2D.Force);
    }

    private void SetDirectionOfAim()
    {
        if (RuntimeEntities.Instance.Camera.ScreenToWorldPoint(InputProcessor.Instance.GetMousePosition()).x < transform.position.x)
        {
            if (_isFacingRight)
            {
                ChangeSide();
                _gun.SetBulletOrigin(_bulletOriginLeft);
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

        else if (RuntimeEntities.Instance.Camera.ScreenToWorldPoint(InputProcessor.Instance.GetMousePosition()).x > transform.position.x)
        {
            if (!_isFacingRight)
            {
                ChangeSide();
                _gun.SetBulletOrigin(_bulletOriginRight);
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

    private bool OnTheGround()
    {
        Debug.DrawRay(_raycastOrigin.position, Vector2.down * _groundCheckRayLength, Color.red);
        bool _onTheGround = Physics2D.Raycast(_raycastOrigin.position, Vector2.down, _groundCheckRayLength);

        if (_isJumping == true && _onTheGround == true)
        {
            _isJumping = false;
            _spriteController.SetStretchAnimatorParameter();
        } else if (_onTheGround == false )
        {
            _isJumping = true;
        }
        return _onTheGround;
    }

    private void ChangeSide()
    {
        _spriteController.FlipWeaponSprite();
        _spriteController.FlipPlayerSprite();
        _isFacingRight = !_isFacingRight;
        _spriteController.SetIsFacingRightAnimatorParameter(_isFacingRight);
    }

    private float GetRotationAngle()
    {
        Vector2 _dir = InputProcessor.Instance.GetMousePosition() - new Vector2(RuntimeEntities.Instance.Camera.WorldToScreenPoint(_arm.position).x, RuntimeEntities.Instance.Camera.WorldToScreenPoint(_arm.position).y);
        float _angle = Mathf.Atan2(_dir.y, _dir.x) * Mathf.Rad2Deg;
        Debug.Log("angle: " + _angle);
        return _angle;
    }
}
