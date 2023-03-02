using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunController : MonoBehaviour
{
    [SerializeField] private Transform _bulletOrigin;
    [SerializeField] private Rigidbody2D _playerRb;
    [SerializeField] private Animator _anim;

    [SerializeField] private int _xBulletScatterOffset;
    [SerializeField] private int _yBulletScatterOffset;

    [SerializeField] private float _recoilForceX;
    [SerializeField] private float _recoilForceY;

    public float RecoilForceX { get { return _recoilForceX; }}
    public float RecoilForceY { get { return _recoilForceY; }}

    private System.Random _rand;

    void Start()
    {
        _rand = new System.Random();
    }

    void Update()
    {
        
    }

    public void Shoot()
    {
        Vector2 _mousePos = InputProcessor.Instance.GetMousePosition();
        float _xOffset = _rand.Next(0, _xBulletScatterOffset);
        float _yOffset = _rand.Next(0, _yBulletScatterOffset);
        Vector2 _target = new Vector2(_mousePos.x + _xOffset, _mousePos.y + _yOffset);
        Vector2 _originOnScreen = RuntimeEntities.Instance.Camera.WorldToScreenPoint(_playerRb.gameObject.transform.position);
        _anim.Play("WeaponTremble");
        Debug.DrawLine(_bulletOrigin.position, RuntimeEntities.Instance.Camera.ScreenToWorldPoint(_target), Color.red);
    }

    public void SetBulletOrigin(Transform origin)
    {
        _bulletOrigin = origin;
    }
}
