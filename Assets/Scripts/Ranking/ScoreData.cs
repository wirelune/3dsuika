using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

[System.Serializable]
public class ScoreData
{
    public List<int> scores = new List<int>();

    private static readonly string saveDir = "data:/";
    private static readonly string fileName = "scores.bin";

    private static List<int> cachedScores = null;

    public static List<int> Load()
    {
        if (cachedScores != null)
            return new List<int>(cachedScores);

        List<int> result = new List<int> { 0, 0, 0 };

#if UNITY_N3DS
        UnityEngine.N3DS.FileSystemSave.Mount();
#endif
        string path = saveDir + fileName;

        if (File.Exists(path))
        {
            BinaryFormatter binaryFormatter = new BinaryFormatter();
            FileStream file = File.Open(path, FileMode.Open, FileAccess.Read);
            ScoreData data = (ScoreData)binaryFormatter.Deserialize(file);
            file.Close();
            result = data.scores;
        }

#if UNITY_N3DS
        UnityEngine.N3DS.FileSystemSave.Unmount();
#endif

        cachedScores = new List<int>(result);
        return new List<int>(result);
    }

    public static void AddScore(int newScore)
    {
        List<int> scores = Load();
        scores.Add(newScore);
        scores.Sort((a, b) => b.CompareTo(a));

        if (scores.Count > 3)
            scores.RemoveRange(3, scores.Count - 3);

        while (scores.Count < 3)
            scores.Add(0);

        cachedScores = new List<int>(scores);

        ScoreData data = new ScoreData();
        data.scores = scores;

#if UNITY_N3DS
        UnityEngine.N3DS.FileSystemSave.Mount();
#endif
        BinaryFormatter binaryFormatter = new BinaryFormatter();
        FileStream file = File.Create(saveDir + fileName);
        binaryFormatter.Serialize(file, data);
        file.Close();

#if UNITY_N3DS
        UnityEngine.N3DS.FileSystemSave.Unmount();
#endif
    }
}
