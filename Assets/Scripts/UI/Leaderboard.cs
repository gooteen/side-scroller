using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class LeaderboardRecord
{
    private string _playerName;
    private int _playerScore = 0;
    private float _playerTime = 0;
    private bool _currentPlayer;

    public string PlayerName { get { return _playerName; } set { _playerName = value; } }
    public int PlayerScore { get { return _playerScore; } set { _playerScore = value; } }
    public float PlayerTime { get { return _playerTime; } set { _playerTime = value; } }
    public bool IsCurrentPlayer { get { return _currentPlayer; } set { _currentPlayer = value; } }

    public LeaderboardRecord(string playerName)
    {
        _playerName = playerName;
    }

    public LeaderboardRecord(string playerName, int playerScore, float playerTime)
    {
        _playerName = playerName;
        _playerScore = playerScore;
        _playerTime = playerTime;
    }
}

[CreateAssetMenu(fileName = "Leaderboard", menuName = "Leaderboard")]
public class Leaderboard : ScriptableObject
{
    [SerializeField] private List<LeaderboardRecord> _records;

    public List<LeaderboardRecord> Records
    {
        get { return _records; }
    }

    public LeaderboardRecord this[string name]
    {
        get { return GetRecordByName(name); } 
    }

    public void Clear()
    {
        _records.Clear();
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

    public void AddNewRecord(LeaderboardRecord record)
    {
        _records.Add(record);
    }

    public void DropRecord(LeaderboardRecord record)
    {
        _records.Remove(record);
    }

    public List<LeaderboardRecord> GetRecordsListSortedByTime()
    {
        List<LeaderboardRecord> sortedRecords = new List<LeaderboardRecord>();
        foreach (LeaderboardRecord _rec in _records)
        {
            sortedRecords.Add(_rec);
        }

        for (int i = 0; i < sortedRecords.Count - 1; i++)
        {
            for (int j = 0; j < sortedRecords.Count - 1 - i; j++)
            {
                if (sortedRecords[j].PlayerTime >= sortedRecords[j+1].PlayerTime)
                {
                    var _temp = sortedRecords[j + 1];
                    sortedRecords[j + 1] = sortedRecords[j];
                    sortedRecords[j] = _temp;
                }
            }
        }

        return sortedRecords;
    }

    public List<LeaderboardRecord> GetRecordsListSortedByScore()
    {
        List<LeaderboardRecord> sortedRecords = new List<LeaderboardRecord>();
        foreach(LeaderboardRecord _rec in _records)
        {
            sortedRecords.Add(_rec);
        }

        for (int i = 0; i < sortedRecords.Count - 1; i++)
        {
            for (int j = 0; j < sortedRecords.Count - 1 - i; j++)
            {
                if (sortedRecords[j].PlayerScore <= sortedRecords[j + 1].PlayerScore)
                {
                    var temp = sortedRecords[j + 1];
                    sortedRecords[j + 1] = sortedRecords[j];
                    sortedRecords[j] = temp;
                }
            }
        }

        return sortedRecords;
    }
}
