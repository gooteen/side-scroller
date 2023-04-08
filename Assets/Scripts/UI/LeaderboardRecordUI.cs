using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;

public class LeaderboardRecordUI : MonoBehaviour
{
    private int _index;
    private string _playerName;
    private int _playerScore;
    private float _playerTime;
    private EventTrigger _eventTrigger;

    public EventTrigger EventTrigger {  get { return _eventTrigger; } }

    public int Index { get { return _index;  } set { _index = value; } }
    public string PlayerName { get { return _playerName;  } set { SetPlayerName(value);  } }
    public int PlayerScore { private get { return _playerScore;  } set { SetPlayerScore(value); ;  } }
    public float PlayerTime { private get { return _playerTime;  } set { SetPlayerTime(value); ;  } }

    [SerializeField] private TMP_Text _playerNameUI;
    [SerializeField] private TMP_Text _playerScoreUI;
    [SerializeField] private TMP_Text _playerTimeUI;

    private void Awake()
    {
        _eventTrigger = GetComponent<EventTrigger>();
    }

    private void SetPlayerScore(int score)
    {
        _playerScore = score;
        _playerScoreUI.text = score.ToString();
    }

    private void SetPlayerName(string name)
    {
        _playerName = name;
        _playerNameUI.text = name;
    }

    private void SetPlayerTime(float time)
    {
        _playerTime = time;
        _playerTimeUI.text = time.ToString();
    }

}
