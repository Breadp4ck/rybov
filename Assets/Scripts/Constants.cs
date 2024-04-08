using System;
using System.Collections.Generic;
using GlobalState.Level;
using JetBrains.Annotations;

public static class Constants
{
    public enum SceneType : byte
    {
        MainMenu,
        Level0,
    }
    
    [CanBeNull]
    public static string GetNextSceneString(SceneType sceneType)
    {
        return sceneType switch
        {
            SceneType.MainMenu => "MainMenu",
            SceneType.Level0 => $"Level_0",
            _ => null
        };
    }
}