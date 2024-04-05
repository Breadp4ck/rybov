using System;
using System.Collections.Generic;
using System.Linq;
using Units.Spawning;
using UnityEngine;

namespace GlobalStates.Game
{
    public class Game : MonoBehaviour
    {
        public static Game Instance { get; private set; }
        
        public event Action<StateType> StateChangedEvent; 
        
        public State CurrentState { get; private set; }
        
        public bool IsGamePaused { get; private set; }
        
        [SerializeField] private float _assaultDurationSeconds;

        [SerializeField] private SpawnersHandler _spawnersHandler;
        
        private IEnumerable<State> _states = Enumerable.Empty<State>();
        
        private void Awake()
        {
            if (Instance != null)
            {
                Destroy(gameObject);
                return;
            }
            
            Instance = this;
        }

        private void Start()
        {
            _states = new List<State>
            {
                new StartState(),
                new AssaultState(_assaultDurationSeconds),
                new FleeingState(_spawnersHandler),
                new FinishState()
            };
            
            ChangeState(StateType.Start);
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape) == false)
            {
                return;
            }

            if (IsGamePaused)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }

        private void OnDestroy()
        {
            Instance = null;
        }

        public void ChangeState(StateType stateType)
        {
            State state = _states.FirstOrDefault(x => x.Type == stateType);
            if (state == null)
            {
                Debug.LogError($"State {stateType} is not settable for {gameObject}.");
                return;
            }

            CurrentState?.Stop();
            CurrentState = state;
            CurrentState.Start();
            
            StateChangedEvent?.Invoke(CurrentState.Type);
        }
        
        private void PauseGame()
        {
            IsGamePaused = true;
            Time.timeScale = 0;
        }

        private void ResumeGame()
        {
            IsGamePaused = false;
            Time.timeScale = 1;
        }
    }

}