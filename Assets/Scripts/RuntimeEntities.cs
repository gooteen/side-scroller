using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.SceneManagement;

public class RuntimeEntities : MonoBehaviour
{
    [SerializeField] private Camera _cam;
    [SerializeField] private PlayerController _player;
    [SerializeField] private PlayerSettings _settings;
    [SerializeField] private List<CinemachineVirtualCamera> _vCams;
    [SerializeField] private CinemachineVirtualCamera _focusCam;

    [SerializeField] private Leaderboard _leaderboard; 

    public static RuntimeEntities Instance
    {
        get; private set;
    }

    public PlayerController Player { get { return _player; } }

    public PlayerSettings Settings { get { return _settings; } }

    public Camera Camera { get { return _cam; } }

    void Awake()
    {
        Instance = this;
        foreach (CinemachineVirtualCamera _vCam in _vCams)
        {
            _vCam.Priority = 0;
        }
        _focusCam.Priority = 0;
    }

    public void Pause()
    {
        Time.timeScale = 0;
        _player.Arm.gameObject.SetActive(false);
        _player.Active = false;
    }

    public void UnPause()
    {
        Time.timeScale = 1;
        _player.Active = true;
        if (InputProcessor.Instance.RightMouseButtonPressed())
        {
            _player.Arm.gameObject.SetActive(true);
        }
    }

    public void UpdatePlayerScore()
    {
         _leaderboard[_settings._currentPlayerName].PlayerScore = _player.Points;
         _leaderboard[_settings._currentPlayerName].PlayerTime = _player.TimeAmount;
        SaveSystem.SaveLeaderboardData(_leaderboard);
    }

    public void ReloadLevel()
    {
        SceneManager.LoadScene(1);
    }

    public void GoToMenu()
    {
        SceneManager.LoadScene(0);
    }

    public void UpdateCurrentCamera(CinemachineVirtualCamera newCam)
    {
        foreach (CinemachineVirtualCamera _vCam in _vCams)
        {
            if (_vCam != newCam)
            {
                _vCam.Priority = 0;
            } else
            {
                _vCam.Priority = 1;
            }
        }
    }

    public void SetFocusCam()
    {
        foreach (CinemachineVirtualCamera _vCam in _vCams)
        {
            _vCam.Priority = 0;
        }
        _focusCam.Priority = 1;
    }
}