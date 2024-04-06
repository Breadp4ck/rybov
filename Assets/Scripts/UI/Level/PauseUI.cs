using System;
using GlobalStates.Game;
using UnityEngine;

public class PauseUI : MonoBehaviour
{
    private Game Game => Game.Instance;

    private void Start()
    {
        gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        Game.PausedEvent += () => gameObject.SetActive(true);
        Game.ResumedEvent += () => gameObject.SetActive(false);
    }

    public void OnResumeButtonClicked()
    {
        Game.ResumeGame();
    }
}
