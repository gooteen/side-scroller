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

    private void Awake()
    {
        SetNormalCursor();
    }

    public void ExitGame()
    {
        Application.Quit();
        Debug.Log("exiting");
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
