using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpriteController : MonoBehaviour
{
    public bool _updateRotation;
    //[SerializeField] private EnemyController; _updateRotation;
    private SpriteRenderer _sprite;
    private Animator _anim;

    public SpriteRenderer Sprite
    {
        get { return _sprite; }
    }

    public Animator Animator
    {
        get { return _anim; }
    }

    void Start()
    {
        _sprite = GetComponent<SpriteRenderer>();
        _anim = GetComponent<Animator>();
    }

    private void Update()
    {
        UpdateSpriteRotation();
    }

    public void UpdateSpriteRotation()
    {
        if (_updateRotation)
        {
            if (RuntimeEntities.Instance.Player.transform.position.x < transform.position.x)
            {
                if (!_sprite.flipX)
                {
                    _sprite.flipX = true;
                }
            }
            else
            {
                if (_sprite.flipX)
                {
                    _sprite.flipX = false;
                }
            }
        }
    }

    public void TriggerShooting()
    {

    }
}
