using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public static class SaveSystem
{
    public static void SaveAudioSettings(AudioSettings settings) 
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "AudioSettings.txt";

        FileStream stream = new FileStream(path, FileMode.Create);
        formatter.Serialize(stream, settings);
        stream.Close();
    }

    public static AudioSettings LoadAudioSettings()
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "AudioSettings.txt";
        if (File.Exists(path))
        {
            FileStream stream = new FileStream(path, FileMode.Open);
            AudioSettings settings = formatter.Deserialize(stream) as AudioSettings;
            stream.Close();
            return settings;
        } else
        {
            return null;
        }
    }

    public static void SaveLeaderboardData(Leaderboard data)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "LeaderboardData.txt";
        LeaderboardData leaderboardData = new LeaderboardData(data);
        FileStream stream = new FileStream(path, FileMode.Create);
        formatter.Serialize(stream, leaderboardData);
        stream.Close();
    }

    public static LeaderboardData LoadLeaderboardData()
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "LeaderboardData.txt";
        if (File.Exists(path))
        {
            FileStream stream = new FileStream(path, FileMode.Open);
            LeaderboardData data = formatter.Deserialize(stream) as LeaderboardData;
            stream.Close();
            return data;
        }
        else
        {
            return null;
        }
    }
}
