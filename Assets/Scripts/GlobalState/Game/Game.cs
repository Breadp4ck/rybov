using System;
using System.Collections.Generic;
using System.Linq;
using Fishing.Pool;
using Units.Spawning;
using UnityEngine;

namespace GlobalStates.Game
{
    public class Game : MonoBehaviour
    {
        public static Game Instance { get; private set; }

        public event Action PausedEvent;
        public event Action ResumedEvent;
        
        public event Action<StateType> StateChangedEvent; 
        
        public State CurrentState { get; private set; }
        
        public bool IsGamePaused { get; private set; }
        
        public float RemainingAssaultTimeSeconds { get; private set; }
        
        [SerializeField] private float _assaultDurationSeconds;
        public float AssaultDurationSeconds => _assaultDurationSeconds;
        
        [SerializeField] private float _fleeingDurationSeconds;
        
        [SerializeField] private List<FishLake> _fishLakes;
        
        private IEnumerable<State> _states = Enumerable.Empty<State>();
        
        private void Awake()
        {
            if (Instance != null)
            {
                Destroy(gameObject);
                return;
            }
            
            Instance = this;

            FishPool.FreeFishes?.Clear();
            FishPool.StolenFishes?.Clear();
        }

        private void OnDestroy()
        {
            CurrentState?.Stop();
        }

        private void OnValidate()
        {
            _fishLakes = FindObjectsOfType<FishLake>().ToList();
        }

        private void Start()
        {
            _states = new List<State>
            {
                new StartState(_fishLakes),
                new AssaultState(_assaultDurationSeconds),
                new FleeingState(_fleeingDurationSeconds),
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

        public void SetRemainingAssaultTime(float remainingAssaultTimeSeconds)
        {
            RemainingAssaultTimeSeconds = remainingAssaultTimeSeconds;
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
        
        public void PauseGame()
        {
            IsGamePaused = true;
            Time.timeScale = 0;
            
            PausedEvent?.Invoke();
        }

        public void ResumeGame()
        {
            IsGamePaused = false;
            Time.timeScale = 1;
            
            ResumedEvent?.Invoke();
        }
    }

}