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

    void Update()
    {
        
    }

    public void SetDirectionAnimatorParameter(Vector2 direction)
    {
        _anim.SetFloat("Direction", direction.x);
    }

    public void SetIsFacingRightAnimatorParameter(bool isFacingRight)
    {
        _anim.SetBool("IsFacingRight", isFacingRight);
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
