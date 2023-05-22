using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class LeaderboardData
{
    public string[] playerNameArray;
    public int[] playerScoreArray;
    public float[] playerTimeArray;

    public LeaderboardData(Leaderboard leaderboard)
    {
        playerNameArray = new string[100];
        playerScoreArray = new int[100];
        playerTimeArray = new float[100];

        int i = 0;
        foreach(LeaderboardRecord record in leaderboard.Records)
        {
            playerNameArray[i] = record.PlayerName;
            playerScoreArray[i] = record.PlayerScore;
            playerTimeArray[i] = record.PlayerTime;
            i++;
        }
    }
}
