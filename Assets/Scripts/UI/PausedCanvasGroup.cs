using System;
using GlobalStates.Game;
using UnityEngine;

namespace UI
{
    public class PausedCanvasGroup : MonoBehaviour, ICanvasGroup
    {
        private Game Game => Game.Instance;
        private CanvasGroup _canvasGroup;
        
        private void Awake()
        {
            _canvasGroup = GetComponent<CanvasGroup>();
            BecomeDisabled();
        }

        private void OnEnable()
        {
            Game.PausedEvent += BecomeEnabled;
            Game.ResumedEvent += BecomeDisabled;
        }

        private void BecomeEnabled()
        {
            _canvasGroup.alpha = 1f;
            _canvasGroup.interactable = true;
            _canvasGroup.blocksRaycasts = true;
        }
        
        private void BecomeDisabled()
        {
            _canvasGroup.alpha = 0f;
            _canvasGroup.interactable = false;
            _canvasGroup.blocksRaycasts = false;
        }
    }
}