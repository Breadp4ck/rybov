using System.Collections.Generic;
using GlobalState.Level;

public static class Constants
{
    public enum SceneType : byte
    {
        MainMenu,
        Level,
    }

    public static Dictionary<SceneType, string> Scenes = new()
    {
        { SceneType.MainMenu, "MainMenu"},
        { SceneType.Level, $"Level_{Level.Instance.NextLevelIndex}"},
    };
}