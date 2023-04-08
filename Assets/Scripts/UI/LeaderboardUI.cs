using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class LeaderboardUI : MonoBehaviour
{
    [SerializeField] private Transform _recordConatiner;
    [SerializeField] private GameObject _recordPrefab;
    [SerializeField] private Leaderboard _leaderboard;
    [SerializeField] private List<GameObject> _recordElements;
    [SerializeField] private Button _playButton;
    [SerializeField] private Button _deleteButton;
    [SerializeField] private Button _addPlayerButton;
    [SerializeField] private GameObject _noPlayersText;
    [SerializeField] private TMP_Text _inputFieldValue;
    [SerializeField] private MainMenuConfigurator _menuController;

    [SerializeField] private string _selectedPlayerName;
    [SerializeField] private int _selectedRecordIndex;

    private bool _isDeleteButtonHover;

    public void ChangeDeleteButtonStatus()
    {
        _isDeleteButtonHover = !_isDeleteButtonHover;
    }

    public void FillLeaderboard()
    {
        Debug.Log("FILLING");
        List<LeaderboardRecord> _records = _leaderboard.GetRecordsListSortedByScore();

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
        _selectedRecordIndex = -1;
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
        _entrySelect.callback.AddListener(delegate { SetSelectedIndexAndName(recordFields); });

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
        _entryDeselect.callback.AddListener(delegate { ClearSelectedIndexAndName(recordFields); });

        _entryDeselect.callback.AddListener(delegate { _playButton.gameObject.SetActive(false); });
        _entryDeselect.callback.AddListener(delegate { DisableDeleteButton(); });

        recordFields.EventTrigger.triggers.Add(_entryDeselect);
    }

    private void DisableDeleteButton()
    {
        if (!_isDeleteButtonHover)
        {
            _deleteButton.gameObject.SetActive(false);
        }
    }

    private void SetDeleteButton(LeaderboardRecordUI recordFields)
    {
        Debug.Log("HEEEYooo");
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

    private void SetSelectedIndexAndName(LeaderboardRecordUI recordData)
    {
        _selectedPlayerName = recordData.PlayerName;
        _selectedRecordIndex = recordData.Index;
    }

    private void ClearSelectedIndexAndName(LeaderboardRecordUI recordData)
    {
        _selectedPlayerName = null;
        _selectedRecordIndex = -1;
    }
}
