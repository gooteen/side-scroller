using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteController : MonoBehaviour
{
    [SerializeField] private SpriteRenderer _weaponRenderer;
    private SpriteRenderer _playerRenderer;

    void Awake()
    {
        _playerRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        
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
