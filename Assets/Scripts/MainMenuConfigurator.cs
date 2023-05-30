using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenuConfigurator : MonoBehaviour
{
    public Texture2D normalCursor;
    public Texture2D handCursor;
    public Vector2 hotSpot;
    public Slider soundSlider;
    public Slider musicSlider;
    public PlayerSettings settings;
    public Leaderboard leaderboard;

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
        leaderboard.Clear();
        LeaderboardData data = SaveSystem.LoadLeaderboardData();
        int i = 0;
        if (data != null)
        {
            while (data.playerNameArray[i] != null)
            {
                leaderboard.AddNewRecord(new LeaderboardRecord(data.playerNameArray[i], data.playerScoreArray[i], data.playerTimeArray[i]));
                i++;
            }
        }
    }

    public void SaveLeaderboardData()
    {
        SaveSystem.SaveLeaderboardData(leaderboard);
    }

    public void SaveSettings()
    {
        AudioSettings settings = new AudioSettings(this.settings.soundLevelSetting, this.settings.musicLevelSetting);
        SaveSystem.SaveAudioSettings(settings);
    }

    public void LoadSettings()
    {
        AudioSettings settings = SaveSystem.LoadAudioSettings();
        if (settings != null)
        {
            this.settings.soundLevelSetting = settings.soundLevelSetting;
            this.settings.musicLevelSetting = settings.musicLevelSetting;
        }
    }

    public void SetNormalCursor()
    {
        Cursor.SetCursor(normalCursor, hotSpot, CursorMode.Auto);
    }

    public void SetHandCursor()
    {
        Cursor.SetCursor(handCursor, hotSpot, CursorMode.Auto);
    }

    public void LoadLevel()
    {
        SceneManager.LoadScene(1);
    }

    public void SetSliders()
    {
        soundSlider.value = settings.soundLevelSetting;
        musicSlider.value = settings.musicLevelSetting;
    }

    public void SaveMusicSliderValue()
    {
        settings.musicLevelSetting = musicSlider.value;
    }

    public void SaveSoundSliderValue()
    {
        settings.soundLevelSetting = soundSlider.value;
    }
}
