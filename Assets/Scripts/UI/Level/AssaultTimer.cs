using System;
using System.Collections;
using GlobalStates.Game;
using TMPro;
using UnityEngine;

namespace UI.Level
{
    public class AssaultTimer : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _text;
        
        private Game Game => Game.Instance;
        private float _remainingAssaultTime;
        private StateType _stateType;

        private void Update()
        {
            while (_remainingAssaultTime > 0f && _stateType == StateType.Assault)
            {
                _remainingAssaultTime -= Time.deltaTime;
            }
            _text.text = $"{_remainingAssaultTime}";
        }

        private void OnEnable()
        {
            Game.StateChangedEvent += GameStateChanged;
        }

        private void GameStateChanged(StateType type)
        { 
            _stateType = type;
            print(_stateType);
            if (_stateType == StateType.Start)
            {
                _remainingAssaultTime = Game.AssaultDurationSeconds;
                print(_remainingAssaultTime);
            }
        }
    }
}