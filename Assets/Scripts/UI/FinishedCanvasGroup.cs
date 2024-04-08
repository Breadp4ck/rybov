﻿using System;
using GlobalStates.Game;
using UnityEngine;

namespace UI
{
    public class FinishedCanvasGroup : MonoBehaviour
    {
        private Game Game => Game.Instance;
        private CanvasGroup _canvasGroup;

        private void Start()
        {
            _canvasGroup = GetComponent<CanvasGroup>();
            _canvasGroup.alpha = 0f;
            _canvasGroup.interactable = false;
            _canvasGroup.blocksRaycasts = false;
        }
        
        private void OnEnable()
        {
            Game.StateChangedEvent += OnStateChanged;
        }

        private void OnStateChanged(StateType stateType)
        {
            if (stateType == StateType.Finish)
            { 
                _canvasGroup.alpha = 1f;
                _canvasGroup.interactable = true;
                _canvasGroup.blocksRaycasts = true;
            }
            else
            {
                _canvasGroup.alpha = 0f;
                _canvasGroup.interactable = false;
                _canvasGroup.blocksRaycasts = false;
            }
        }
    }
}