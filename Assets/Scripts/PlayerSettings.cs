using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerSettings", menuName = "PlayerSettings")]
public class PlayerSettings : ScriptableObject
{
    public float playerMovementSpeedGround;
    public float playerMovementSpeedAir;
    public float jumpImpulse;
    public float bulletDamage;
    public string currentPlayerName;
    public float soundLevelSetting;
    public float musicLevelSetting;
}
