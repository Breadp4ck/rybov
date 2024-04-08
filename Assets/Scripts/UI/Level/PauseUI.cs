using System;
using GlobalStates.Game;
using SceneManagement.SceneManagement;
using UnityEngine;

public class PauseUI : MonoBehaviour
{
    private Game Game => Game.Instance;
    private CanvasGroup _canvasGroup;

    private void Start()
    {
        _canvasGroup = GetComponent<CanvasGroup>();
        BecomeDisabled();
    }

    private void OnEnable()
    {
        Game.PausedEvent += BecomeEnabled;
        Game.ResumedEvent += BecomeDisabled;
    }

    public void OnResumeButtonClicked()
    {
        Game.ResumeGame();
    }

    public async void OnRestartButtonClicked()
    {
        await SceneChanger.ChangeSceneAsync(Constants.SceneType.Level0);
    }

    public async void OnMainMenuButtonClicked()
    {
        await SceneChanger.ChangeSceneAsync(Constants.SceneType.MainMenu);
    }

    private void BecomeEnabled()
    {
        _canvasGroup.alpha = 1f;
        _canvasGroup.interactable = true;
    }
    
    private void BecomeDisabled()
    {
        _canvasGroup.alpha = 0f;
        _canvasGroup.interactable = false;
    }
}
