using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunController : MonoBehaviour
{
    [SerializeField] private Transform _bulletOrigin;
    [SerializeField] private Transform _arm;
    [SerializeField] private GameObject _bulletPrefab;
    [SerializeField] private Rigidbody2D _playerRb;
    [SerializeField] private Animator _anim;

    [SerializeField] private int _xBulletScatterOffset;
    [SerializeField] private int _yBulletScatterOffset;

    [SerializeField] private float _recoilForceX;
    [SerializeField] private float _recoilForceY;

    [SerializeField] private float _timeBetweenShots;

    [SerializeField] private int maxTrajectoryAngleOffset;

    [SerializeField] private float _releaseTime;


    private System.Random _rand;

    public float RecoilForceX { get { return _recoilForceX; }}
    public float RecoilForceY { get { return _recoilForceY; }}


    void Start()
    {
        _rand = new System.Random();
        ResetReleaseTime();
    }

    public void Shoot()
    {
        _anim.Play("WeaponTremble");
        if (Time.time - _releaseTime >= _timeBetweenShots)
        {
            float rotationZ = _rand.Next(-maxTrajectoryAngleOffset, maxTrajectoryAngleOffset);
            GameObject _bullet = Instantiate(_bulletPrefab, _bulletOrigin.position, _bulletOrigin.rotation);
            _bullet.transform.eulerAngles = new Vector3(_arm.eulerAngles.x, _arm.eulerAngles.y, _arm.eulerAngles.z + rotationZ);
            _releaseTime = Time.time;
        } else
        {
            Debug.Log("pause");
        }
    }

    public void SetBulletOrigin(Transform origin)
    {
        _bulletOrigin = origin;
    }

    public void ResetReleaseTime()
    {
        _releaseTime = 0;
    }
}
