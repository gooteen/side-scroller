﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;
using UnityEngine.SceneManagement;

public class LeaderboardUI : MonoBehaviour
{
    public enum FilteringMode {Time, Score };

    [SerializeField] private Transform _recordConatiner;
    [SerializeField] private GameObject _recordPrefab;
    [SerializeField] private Leaderboard _leaderboard;
    [SerializeField] private List<GameObject> _recordElements;

    [SerializeField] private Button _playButton;
    [SerializeField] private Button _deleteButton;
    [SerializeField] private Button _addPlayerButton;

    [SerializeField] private Color _filterStartColor;
    [SerializeField] private TMP_Text _scoreFilterText;
    [SerializeField] private TMP_Text _timeFilterText;

    [SerializeField] private GameObject _noPlayersText;
    [SerializeField] private TMP_Text _inputFieldValue;
    [SerializeField] private MainMenuConfigurator _menuController;
    [SerializeField] private PlayerSettings _playerSettngs;

    [SerializeField] private string _selectedPlayerName;
    //[SerializeField] private int _selectedRecordIndex;

    private bool _isDeleteButtonHover;
    private FilteringMode _currentFilteringMode;
    [SerializeField] private bool _isPlayButtonHover;

    private void Awake()
    {
        _currentFilteringMode = FilteringMode.Score;
        _playerSettngs._currentPlayerName = null;
        _selectedPlayerName = null;
    }

    public void EnterTheGame()
    {
        if (_selectedPlayerName.Length > 1)
        {
            _playerSettngs._currentPlayerName = _selectedPlayerName;
            SceneManager.LoadScene(1);
        }
    }

    public void ChangeDeleteButtonStatus()
    {
        _isDeleteButtonHover = !_isDeleteButtonHover;
    }

    public void ChangePlayButtonStatus()
    {
        _isPlayButtonHover = !_isPlayButtonHover;
    }

    public void SetFilteringMode(int mode)
    {
        if (mode == 1)
        {
            _currentFilteringMode = FilteringMode.Score;
        } else
        {
            _currentFilteringMode = FilteringMode.Time;
        }
    }

    public void FillLeaderboard()
    {
        List<LeaderboardRecord> _records;
        if (_currentFilteringMode == FilteringMode.Score)
        {
            _records = _leaderboard.GetRecordsListSortedByScore();
            _scoreFilterText.color = Color.white;
            _timeFilterText.color = _filterStartColor;
        } else
        {
            _records = _leaderboard.GetRecordsListSortedByTime();
            _timeFilterText.color = Color.white;
            _scoreFilterText.color = _filterStartColor;
        }


        if (_records.Count == 0)
        {
            _noPlayersText.SetActive(true);
        }
        else
        {
            for (int i = 0; i < _records.Count; i++)
            {
                _noPlayersText.SetActive(false);
                GameObject _newRecordElement = Instantiate(_recordPrefab, _recordConatiner);
                LeaderboardRecordUI _recordFields = _newRecordElement.GetComponent<LeaderboardRecordUI>();
                _recordElements.Add(_newRecordElement);
                _recordFields.Index = i;
                _recordFields.PlayerName = _records[i].PlayerName;
                _recordFields.PlayerScore = _records[i].PlayerScore;
                _recordFields.PlayerTime = _records[i].PlayerTime;

                //setting up the on-select action

                ConfigureOnSelectActionForRecord(_recordFields);

                //setting up the on-deselect action

                ConfigureOnDeselectActionForRecord(_recordFields);
            }
        }
        _selectedPlayerName = null;
    }

    public void ClearLeaderboard()
    {
        foreach (GameObject _element in _recordElements)
        {
            Destroy(_element);
        }
        _recordElements.Clear();
    }

    public void CheckAddButton()
    {
        if (_inputFieldValue.text.Length > 1)
        {
            _addPlayerButton.enabled = true;
        } else
        {
            _addPlayerButton.enabled = false;
        }
    }

    public void CreatePlayer()
    {
        _leaderboard.AddNewRecord(_inputFieldValue.text);
    }

    private void OnEnable()
    {
        _isDeleteButtonHover = false;
        FillLeaderboard();
    }

    private void OnDisable()
    {
        ClearLeaderboard();
    }

    private void ConfigureOnSelectActionForRecord(LeaderboardRecordUI recordFields)
    {
        EventTrigger.Entry _entrySelect = new EventTrigger.Entry();
        _entrySelect.eventID = EventTriggerType.Select;
        _entrySelect.callback.RemoveAllListeners();
        _entrySelect.callback.AddListener(delegate { SetSelectedName(recordFields); });

        _entrySelect.callback.AddListener(delegate { _playButton.gameObject.SetActive(true); });
        _entrySelect.callback.AddListener(delegate { _deleteButton.gameObject.SetActive(true); });

        recordFields.EventTrigger.triggers.Add(_entrySelect);

        _entrySelect.callback.AddListener(delegate { SetDeleteButton(recordFields); });
    }

    private void ConfigureOnDeselectActionForRecord(LeaderboardRecordUI recordFields)
    {
        EventTrigger.Entry _entryDeselect = new EventTrigger.Entry();
        _entryDeselect.eventID = EventTriggerType.Deselect;
        _entryDeselect.callback.RemoveAllListeners();

        _entryDeselect.callback.AddListener(delegate { DisableDeleteButton(); });
        _entryDeselect.callback.AddListener(delegate { DisablePlayButton(); });

        recordFields.EventTrigger.triggers.Add(_entryDeselect);
    }

    private void DisableDeleteButton()
    {
        if (!_isDeleteButtonHover)
        {
            _deleteButton.gameObject.SetActive(false);
        }
    }

    private void DisablePlayButton()
    {
        if (!_isPlayButtonHover)
        {
            _playButton.gameObject.SetActive(false);
            ClearSelectedName();
        }
    }

    private void SetDeleteButton(LeaderboardRecordUI recordFields)
    {
        _deleteButton.onClick.RemoveAllListeners();
        //_deleteButton.onClick.RemoveAllListeners();
        _deleteButton.onClick.AddListener(delegate { _leaderboard.DropRecord(_leaderboard[recordFields.PlayerName]); });
        _deleteButton.onClick.AddListener(ClearLeaderboard);
        _deleteButton.onClick.AddListener(FillLeaderboard);
        _deleteButton.onClick.AddListener(delegate { _deleteButton.gameObject.SetActive(false); });
        _deleteButton.onClick.AddListener(ChangeDeleteButtonStatus);
        _deleteButton.onClick.AddListener(delegate { _menuController.SetNormalCursor(); }); 
    }

    /*
    private void ClearDeleteButton()
    {

    }
    */

    private void SetSelectedName(LeaderboardRecordUI recordData)
    {
        _selectedPlayerName = recordData.PlayerName;
    }

    private void ClearSelectedName()
    {
        _selectedPlayerName = null;
    }
}
