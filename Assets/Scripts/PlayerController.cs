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
    [SerializeField] private int _currentPointCount;

    [SerializeField] private float _heatUpStepPerFrame = 0.1f;
    [SerializeField] private float _heatDownStepPerFrame = 0.1f;

    [SerializeField] private bool _isShooting;

    private float _startTime;
    private float _currentTime;

    private CapsuleCollider2D _col;
    private bool _isJumping;
    private bool _isAiming;
    private bool _armVisible;
    private bool _active;
    private bool _inLava;
    
    private float _coolDownStartTime;
    private float _currentHealth;

    private Rigidbody2D _rb;

    public Rigidbody2D Rigidbody
    {
        get { return _rb; }
    }

    public GunController Gun
    {
        get { return _gun; }
    }

    public SpriteController Sprite
    {
        get { return _spriteController; }
    }

    public bool InLava
    {
        get { return _inLava; }
        set { _inLava = value; }
    }

    public int Points
    {
        get { return _currentPointCount; }
    }

    public int TimeAmount
    {
        get { return (int)_currentTime; }
    }

    public bool Active
    {
        set { _active = value; }
    }

    public Transform Arm
    {
        get { return _arm; }
    }

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _col = GetComponent<CapsuleCollider2D>();
        _startTime = Time.time;
    }

    private void Start()
    {
        _active = true;
        _currentPointCount = 0;
        _currentHealth = _maxHealth;
        UIController.Instance.UpdateHealthScaleFillAmount(_currentHealth / _maxHealth);
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
        _currentTime = Time.time - _startTime;
        UIController.Instance.Timer.text = ((int)_currentTime).ToString();

        if (_active)
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
                            if (_gun.isActiveAndEnabled)
                            {
                                _gun.StartEmittingSmoke();
                            }
                            _isShooting = false;
                        }
                    }
                }
                else
                {
                    if (_gun.isActiveAndEnabled)
                    {
                        _gun.StartEmittingSmoke();
                    }
                    _isShooting = false;
                }
            }
            else
            {
                _isShooting = false;
            }

            if (!_isShooting)
            {
                _gun.DecreaseHeat(_heatDownStepPerFrame);
            }

            Move();

            if (OnTheGround())
            {
                Jump();
            }
        }
    }

    public void AddPoint()
    {
        _currentPointCount += 1;
        UIController.Instance.Counter.text = _currentPointCount.ToString();
    }

    public void PushRb(Vector2 dir, float force)
    {
        _rb.AddForce(dir.normalized * force, ForceMode2D.Impulse);
    }

    public void TakeDamage(float damage)
    {
        if (_active)
        {
            _currentHealth -= damage;
            UIController.Instance.UpdateHealthScaleFillAmount(_currentHealth / _maxHealth);
            _spriteController.PlayDamageImpactAnimation();
            if (_currentHealth <= 0)
            {
                Die();
                SoundSystem.Instance.PlayEffect("death");
            }
            else
            {
                SoundSystem.Instance.PlayRandomEffect("PlayerDamage");
            }
        }
    }

    public void Heal(float amount)
    {
        _currentHealth += amount;
        if (_currentHealth > _maxHealth)
        {
            _currentHealth = _maxHealth;
        }
        UIController.Instance.UpdateHealthScaleFillAmount(_currentHealth / _maxHealth);
    }

    public void Die()
    {
        _spriteController.SetDeathTrigger();
        _spriteController.Renderer.sortingOrder = 100;
        UIController.Instance.StartCoroutine("FadeOut");
        _col.enabled = false;
        _active = false;
        _rb.isKinematic = true;
        _rb.velocity = Vector2.zero;
        _arm.gameObject.SetActive(false);
        RuntimeEntities.Instance.SetFocusCam();
        UIController.Instance.StartCoroutine("SpellDeathMessage");
        UIController.Instance.ShowButtons(0);
        UIController.Instance.SetNormalPointer();
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
        _arm.localRotation = Quaternion.AngleAxis(GetRotationAngle(), Vector3.forward);
        SetDirectionOfAim();
    }

    private void Jump()
    {
        if (InputProcessor.Instance.JumpButtonPressed())
        {
            _rb.AddForce(Vector2.up * _settings.jumpImpulse, ForceMode2D.Impulse);
            _spriteController.SetStretchAnimatorParameter();
            SoundSystem.Instance.PlayEffect("jump");
        }
    }

    private void Move()
    {
        Vector2 _direction = InputProcessor.Instance.GetMovementDirection().normalized;
        
        if (_direction.magnitude != 0)
        {
            if (OnTheGround())
            {
                _rb.velocity = new Vector2(_direction.x * _settings.playerMovementSpeedGround, _rb.velocity.y);
            } else
            {
                if (!_isAiming)
                {
                    _rb.velocity = new Vector2(_direction.x * _settings.playerMovementSpeedGround, _rb.velocity.y);
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
                _arm.localRotation = Quaternion.Euler(_arm.localEulerAngles.x, _arm.localEulerAngles.y, 270 - _weaponRotationLimitAngle);
            }
        }

        else if (RuntimeEntities.Instance.Camera.ScreenToWorldPoint(InputProcessor.Instance.GetMousePosition()).x >= transform.position.x)
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
                _arm.localRotation = Quaternion.Euler(_arm.localEulerAngles.x, _arm.localEulerAngles.y, _weaponRotationLimitAngle);
            }
        }
    }

    private bool OnTheGround()
    {
        Debug.DrawRay(_raycastOrigin.position, Vector2.down * _groundCheckRayLength, Color.red);
        bool onTheGround = Physics2D.Raycast(_raycastOrigin.position, Vector2.down, _groundCheckRayLength, LayerMask.GetMask("Ground"));
        _spriteController.SetOnTheGroundAnimatorParameter(onTheGround);
        if (_isJumping == true && onTheGround == true)
        {
            _isJumping = false;
            _spriteController.SetStretchAnimatorParameter();
        } else if (onTheGround == false )
        {
            _isJumping = true;
        }
        return onTheGround;
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
        Vector2 dir = InputProcessor.Instance.GetMousePosition() - new Vector2(RuntimeEntities.Instance.Camera.WorldToScreenPoint(_arm.position).x, RuntimeEntities.Instance.Camera.WorldToScreenPoint(_arm.position).y);
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        return angle;
    }
}
