using System;
using System.Collections.Generic;
using GlobalState.Level;
using JetBrains.Annotations;

public static class Constants
{
    public enum SceneType : byte
    {
        MainMenu,
        Level,
    }
    
    [CanBeNull]
    public static string GetNextSceneString(SceneType sceneType)
    {
        return sceneType switch
        {
            SceneType.MainMenu => "MainMenu",
            SceneType.Level => $"Level_{GetNextLevelIndex()}",
            _ => null
        };
    }

    private static uint GetNextLevelIndex()
    {
        return Level.Instance.NextLevelIndex;
    }
}