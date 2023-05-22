using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class AudioSettings
{
    public float _soundLevelSetting;
    public float _musicLevelSetting;

    public AudioSettings(float soundLevel, float musicLevel)
    {
        _soundLevelSetting = soundLevel;
        _musicLevelSetting = musicLevel;
    }
}
