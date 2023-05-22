using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenuConfigurator : MonoBehaviour
{
    public Texture2D _normalCursor;
    public Texture2D _handCursor;
    public Vector2 _hotSpot;
    public Slider _soundSlider;
    public Slider _musicSlider;
    public PlayerSettings _settings;
    public Leaderboard _leaderboard;

    private void Awake()
    {
        SetNormalCursor();
        LoadSettings();
        LoadLeaderboardData();
    }

    public void ExitGame()
    {
        SaveSettings();
        Application.Quit();
    }

    public void LoadLeaderboardData()
    {
        _leaderboard.Clear();
        LeaderboardData data = SaveSystem.LoadLeaderboardData();
        int i = 0;
        if (data != null)
        {
            while (data.playerNameArray[i] != null)
            {
                _leaderboard.AddNewRecord(new LeaderboardRecord(data.playerNameArray[i], data.playerScoreArray[i], data.playerTimeArray[i]));
                i++;
            }
        }
    }

    public void SaveLeaderboardData()
    {
        SaveSystem.SaveLeaderboardData(_leaderboard);
    }

    public void SaveSettings()
    {
        AudioSettings settings = new AudioSettings(_settings._soundLevelSetting, _settings._musicLevelSetting);
        SaveSystem.SaveAudioSettings(settings);
    }

    public void LoadSettings()
    {
        AudioSettings settings = SaveSystem.LoadAudioSettings();
        if (settings != null)
        {
            _settings._soundLevelSetting = settings._soundLevelSetting;
            _settings._musicLevelSetting = settings._musicLevelSetting;
        }
    }

    public void SetNormalCursor()
    {
        Cursor.SetCursor(_normalCursor, _hotSpot, CursorMode.Auto);
    }

    public void SetHandCursor()
    {
        Cursor.SetCursor(_handCursor, _hotSpot, CursorMode.Auto);
    }

    public void LoadLevel()
    {
        SceneManager.LoadScene(1);
    }

    public void SetSliders()
    {
        _soundSlider.value = _settings._soundLevelSetting;
        _musicSlider.value = _settings._musicLevelSetting;
    }

    public void SaveMusicSliderValue()
    {
        _settings._musicLevelSetting = _musicSlider.value;
    }

    public void SaveSoundSliderValue()
    {
        _settings._soundLevelSetting = _soundSlider.value;
    }
}
