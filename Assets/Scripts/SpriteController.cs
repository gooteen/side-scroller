using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteController : MonoBehaviour
{
    [SerializeField] private SpriteRenderer _weaponRenderer;
    private SpriteRenderer _playerRenderer;
    private Animator _anim;

    void Awake()
    {
        _anim = GetComponent<Animator>();
        _playerRenderer = GetComponent<SpriteRenderer>();
    }

    public void PlayDamageImpactAnimation()
    {
        _anim.Play("DamageImpactPlayer");
    }

    public void SetDeathTrigger()
    {
        _anim.SetTrigger("Dead");
    }

    public void SetOnTheGroundAnimatorParameter(bool _grounded)
    {
        _anim.SetBool("OnTheGround", _grounded);
    }

    public void SetYVelocityAnimatorParameter(float _yVelocity)
    {
        _anim.SetFloat("YVelocity", _yVelocity);
    }

    public void SetStretchAnimatorParameter()
    {
        _anim.SetTrigger("Stretch");
    }

    public void SetDirectionAnimatorParameter(Vector2 direction)
    {
        _anim.SetFloat("Direction", direction.x);
    }

    public void SetIsFacingRightAnimatorParameter(bool isFacingRight)
    {
        _anim.SetBool("IsFacingRight", isFacingRight);
    }

    public void SetIsJumpingAnimatorParameter(bool isJumping)
    {
        _anim.SetBool("IsJumping", isJumping);
    }

    public void FlipWeaponSprite()
    {
        _weaponRenderer.flipY = !_weaponRenderer.flipY;
    }

    public void FlipPlayerSprite()
    {
        _playerRenderer.flipX = !_playerRenderer.flipX;
    }


}
