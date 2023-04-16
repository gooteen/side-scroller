using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class LeaderboardRecord
{
    // make the fields private later

    public string _playerName;
    public int _playerScore = 0;
    public float _playerTime = 0;
    public bool _currentPlayer;

    public string PlayerName { get { return _playerName; } set { _playerName = value; } }
    public int PlayerScore { get { return _playerScore; } set { _playerScore = value; } }
    public float PlayerTime { get { return _playerTime; } set { _playerTime = value; } }
    public bool IsCurrentPlayer { get { return _currentPlayer; } set { _currentPlayer = value; } }

    public LeaderboardRecord(string playerName)
    {
        _playerName = playerName;
    }
}

[CreateAssetMenu(fileName = "Leaderboard", menuName = "Leaderboard")]
public class Leaderboard : ScriptableObject
{
    [SerializeField] private List<LeaderboardRecord> _records;

    
    public LeaderboardRecord this[string name]
    {
        get { return GetRecordByName(name); } 
    }
    
    public LeaderboardRecord GetRecordByName(string name)
    {
        foreach (LeaderboardRecord _record in _records)
        {
            if (_record.PlayerName == name)
            {
                return _record;
            }
        }
        return null;
    }

    public void AddNewRecord(string name)
    {
        foreach(LeaderboardRecord rec in _records)
        {
            if (rec.PlayerName == name)
            {
                return;
            }
        }
        _records.Add(new LeaderboardRecord(name));
    }

    public void DropRecord(LeaderboardRecord record)
    {
        _records.Remove(record);
    }

    public List<LeaderboardRecord> GetRecordsListSortedByTime()
    {
        List<LeaderboardRecord> _sortedRecords = new List<LeaderboardRecord>();
        foreach (LeaderboardRecord _rec in _records)
        {
            _sortedRecords.Add(_rec);
        }

        for (int i = 0; i < _sortedRecords.Count - 1; i++)
        {
            for (int j = 0; j < _sortedRecords.Count - 1 - i; j++)
            {
                if (_sortedRecords[j].PlayerTime >= _sortedRecords[j+1].PlayerTime)
                {
                    var _temp = _sortedRecords[j + 1];
                    _sortedRecords[j + 1] = _sortedRecords[j];
                    _sortedRecords[j] = _temp;
                }
            }
        }

        return _sortedRecords;
    }

    public List<LeaderboardRecord> GetRecordsListSortedByScore()
    {
        List<LeaderboardRecord> _sortedRecords = new List<LeaderboardRecord>();
        foreach(LeaderboardRecord _rec in _records)
        {
            _sortedRecords.Add(_rec);
        }

        for (int i = 0; i < _sortedRecords.Count - 1; i++)
        {
            for (int j = 0; j < _sortedRecords.Count - 1 - i; j++)
            {
                if (_sortedRecords[j].PlayerScore <= _sortedRecords[j + 1].PlayerScore)
                {
                    var _temp = _sortedRecords[j + 1];
                    _sortedRecords[j + 1] = _sortedRecords[j];
                    _sortedRecords[j] = _temp;
                }
            }
        }

        return _sortedRecords;
    }
}
