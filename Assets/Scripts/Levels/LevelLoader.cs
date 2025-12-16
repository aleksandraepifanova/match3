using UnityEngine;

public static class LevelLoader
{
    public static LevelData Load(int levelIndex)
    {
        var textAsset = Resources.Load<TextAsset>($"Levels/level_{levelIndex}");
        if (textAsset == null) return null;
        return JsonUtility.FromJson<LevelData>(textAsset.text);
    }
}
