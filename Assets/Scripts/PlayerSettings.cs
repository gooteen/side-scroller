using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerSettings", menuName = "PlayerSettings")]
public class PlayerSettings : ScriptableObject
{
    public float _playerMovementSpeedGround;
    public float _playerMovementSpeedAir;
    public float _jumpImpulse;
}
