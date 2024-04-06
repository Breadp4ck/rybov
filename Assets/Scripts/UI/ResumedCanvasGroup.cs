using GlobalStates.Game;
using UnityEngine;

namespace UI
{
    public class ResumedCanvasGroup : MonoBehaviour, ICanvasGroup
    {
        private Game Game => Game.Instance;
        private CanvasGroup _canvasGroup;
        
        private void Start()
        {
            _canvasGroup = GetComponent<CanvasGroup>();
            BecomeEnabled();
        }
        
        private void OnEnable()
        {
            Game.PausedEvent += BecomeDisabled;
            Game.ResumedEvent += BecomeEnabled;
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
}