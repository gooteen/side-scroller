﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunController : MonoBehaviour
{
    [SerializeField] private Transform _bulletOrigin;
    [SerializeField] private Transform _arm;
    [SerializeField] private GameObject _bulletPrefab;
    [SerializeField] private Rigidbody2D _playerRb;
    [SerializeField] private Animator _anim;
    [SerializeField] private Animator _effectAnim;
    [SerializeField] private ParticleSystem _smokeEffect;

    [SerializeField] private int _xBulletScatterOffset;
    [SerializeField] private int _yBulletScatterOffset;

    [SerializeField] private float _recoilForceX;
    [SerializeField] private float _recoilForceY;

    [SerializeField] private float _timeBetweenShots;

    [SerializeField] private int _maxTrajectoryAngleOffset;

    [SerializeField] private float _releaseTime;

    [SerializeField] private float _overheatValue;
    [SerializeField] private float _currentHeatValue;

    [SerializeField] private float _timeToEmitSmokeFor;

    private Coroutine _currentSmokeEffectCoroutine;
    private ParticleSystem.EmissionModule _smokeEffectEmission;
    private SpriteRenderer _rend;
    private bool _overheated;
    private System.Random _rand;

    public bool Overheated { get { return _overheated; }}
    public float RecoilForceX { get { return _recoilForceX; }}
    public float RecoilForceY { get { return _recoilForceY; }}


    void Awake()
    {
        _smokeEffectEmission = _smokeEffect.emission;
        _rend = _anim.GetComponent<SpriteRenderer>();
        _currentHeatValue = 0;
        _rand = new System.Random();
        ResetReleaseTime();
    }

    public void Shoot()
    {
        _anim.Play("WeaponTremble");
        _effectAnim.Play("Flash");
        if (Time.time - _releaseTime >= _timeBetweenShots)
        {
            SoundSystem.Instance.PlayRandomEffect("Shots");
            float rotationZ = _rand.Next(-_maxTrajectoryAngleOffset, _maxTrajectoryAngleOffset);
            GameObject bullet = Instantiate(_bulletPrefab, _bulletOrigin.position, _bulletOrigin.rotation);
            bullet.transform.eulerAngles = new Vector3(_arm.eulerAngles.x, _arm.eulerAngles.y, _arm.eulerAngles.z + rotationZ);
            _releaseTime = Time.time;
        } 
    }

    public void TakeAwayHeat(float amount)
    {
        _currentHeatValue -= amount;
        if (_currentHeatValue < 0)
        {
            _currentHeatValue = 0;
            if (_overheated)
            {
                ToggleOverheatedValue();
                UIController.Instance.ChangeHeatScaleColor(UIController.ScaleColor.Normal);
            }
        }
    }

    public void DecreaseHeat(float step)
    {
        _currentHeatValue -= step * Time.deltaTime;

        if (_currentHeatValue < 0) 
        {
            _currentHeatValue = 0;
            if (_overheated)
            {
                ToggleOverheatedValue();
                UIController.Instance.ChangeHeatScaleColor(UIController.ScaleColor.Normal);
            }
        }
        _rend.color = new Color(_rend.color.r, (1 - _currentHeatValue/ _overheatValue), (1 - _currentHeatValue / _overheatValue));
        if (_rend.color.g >= 1)
        {
            _rend.color = new Color(_rend.color.r, 1, 1);
        }
        UIController.Instance.UpdateHeatScaleFillAmount(_currentHeatValue / _overheatValue);
    }

    public void IncreaseHeat(float step)
    {
        _currentHeatValue += step * Time.deltaTime;
        if (_currentHeatValue >= _overheatValue)
        {
            _currentHeatValue = _overheatValue;
            if (!_overheated)
            {
                ToggleOverheatedValue();
                UIController.Instance.ChangeHeatScaleColor(UIController.ScaleColor.Red);
            }
        }
        _rend.color = new Color(_rend.color.r, (1 - _currentHeatValue / _overheatValue), (1 - _currentHeatValue / _overheatValue));
        if (_rend.color.g <= 0)
        {
            _rend.color = new Color(_rend.color.r, 0, 0);
        }
        UIController.Instance.UpdateHeatScaleFillAmount(_currentHeatValue / _overheatValue);

    }

    public void ToggleOverheatedValue()
    {
        _overheated = !_overheated;
    }

    public void SetBulletOrigin(Transform origin)
    {
        _bulletOrigin = origin;
        _effectAnim.transform.position = origin.position;
        _smokeEffect.transform.position = origin.position;
    }

    public void ResetReleaseTime()
    {
        _releaseTime = 0;
    }

    public void StartEmittingSmoke()
    {
        if (gameObject)
        {
            _currentSmokeEffectCoroutine = StartCoroutine("EmitSmoke");
        }
    }

    public void StopEmittingSmoke()
    {
        _smokeEffectEmission.enabled = false;
        StopCoroutine("EmitSmoke");
        _currentSmokeEffectCoroutine = null;
    }

    private IEnumerator EmitSmoke()
    {
        _smokeEffectEmission.enabled = true;
        yield return new WaitForSeconds(_timeToEmitSmokeFor);
        _smokeEffectEmission.enabled = false;
    }
}
