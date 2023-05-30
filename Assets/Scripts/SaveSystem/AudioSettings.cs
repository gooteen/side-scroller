using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class AudioSettings
{
    public float soundLevelSetting;
    public float musicLevelSetting;

    public AudioSettings(float soundLevel, float musicLevel)
    {
        soundLevelSetting = soundLevel;
        musicLevelSetting = musicLevel;
    }
}
