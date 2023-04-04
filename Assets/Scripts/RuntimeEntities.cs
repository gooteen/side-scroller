using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class RuntimeEntities : MonoBehaviour
{
    [SerializeField] private Camera _cam;
    [SerializeField] private PlayerController _player;
    [SerializeField] private PlayerSettings _settings;
    [SerializeField] private List<CinemachineVirtualCamera> _vCams;
    [SerializeField] private CinemachineVirtualCamera _deathCam;

    [SerializeField] private Leaderboard _TEMPORARY; 

    public static RuntimeEntities Instance
    {
        get; private set;
    }

    public PlayerController Player { get { return _player; } }

    public PlayerSettings Settings { get { return _settings; } }

    public Camera Camera { get { return _cam; } }

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

    public void SetDeathCam()
    {
        foreach (CinemachineVirtualCamera _vCam in _vCams)
        {
            _vCam.Priority = 0;
        }
        _deathCam.Priority = 1;
    }

    void Awake()
    {
        Instance = this;
        foreach (CinemachineVirtualCamera _vCam in _vCams)
        {
            _vCam.Priority = 0;
        }
        _deathCam.Priority = 0;

        _TEMPORARY.AddNewRecord("Kenya");
        /*
        List<LeaderboardRecord> _records = _TEMPORARY.GetRecordsListSortedByScore();
        List<LeaderboardRecord> _records2 = _TEMPORARY.GetRecordsListSortedByTime();
        
        for (int i = _records.Count - 1; i >= 0; i--)
        {
            Debug.Log($"by score: {i} - {_records[i].PlayerName}, {_records[i].PlayerScore} ");
        }

        for (int i = _records2.Count - 1; i >= 0; i--)
        {
            Debug.Log($"by time: {i} - {_records2[i].PlayerName}, {_records2[i].PlayerTime} ");
        }
        */
    }
}