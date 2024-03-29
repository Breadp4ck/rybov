using System;
using System.Collections.Generic;
using Inputs;
using UnityEngine;

namespace Movement.Cat
{
    #region States

    public enum StateType
    {
        PilferFish,
        Stunned,
        RunawayWithFish
    }

    public class PilferFishState : MovementState
    {
        private readonly float _speed;
        
        public PilferFishState(IStateMachine stateMachine, float speed) : base(stateMachine)
        {
            _speed = speed;
        }

        public override void Start()
        {
            base.Start();
        }

        public override void Update()
        {
            base.Update();
        }

        public override void TryChangeState()
        {
            base.TryChangeState();
        }
    }
    
    public class StunnedState : MovementState
    {
        private readonly TimeSpan _duration;
        
        public StunnedState(IStateMachine stateMachine, TimeSpan duration) : base(stateMachine)
        {
            _duration = duration;
        }

        public override void Start()
        {
            base.Start();
        }

        public override void Update()
        {
            base.Update();
        }

        public override void TryChangeState()
        {
            base.TryChangeState();
        }
    }

    public class RunawayWithFishState : MovementState
    {
        private readonly float _speed;
        
        public RunawayWithFishState(IStateMachine stateMachine, float speed) : base(stateMachine)
        {
            _speed = speed;
        }

        public override void Start()
        {
            base.Start();
        }

        public override void Update()
        {
            base.Update();
        }

        public override void TryChangeState()
        {
            base.TryChangeState();
        }
    }
    
    #endregion
    
    
    public class CatMovementStateMachine : MonoBehaviour, IStateMachine
    {
        public MovementState CurrentState { get; private set; }
        
        public event Action OnStateChangedEvent;
    
        public Transform ManagedTransform => _managedTransform;
        [SerializeField] private Transform _managedTransform;
        
        [Header("Walk")]
        [SerializeField] private float _walkSpeed;
        
        private TimeSpan _stunDuration = TimeSpan.FromSeconds(1);

        private Dictionary<StateType, MovementState> _states = new();

        private void Start()
        {
            // Init states.
            _states = new Dictionary<StateType, MovementState>()
            {
                { StateType.PilferFish, new PilferFishState(this, _walkSpeed) },
                { StateType.Stunned, new StunnedState(this, _stunDuration) },
                { StateType.RunawayWithFish, new RunawayWithFishState(this, _walkSpeed) }
            };
        }

        private void Update()
        {
            CurrentState?.Update();
            CurrentState?.TryChangeState();
        }
        
        public void ChangeState(StateType stateType)
        {
            if (_states.TryGetValue(stateType, out MovementState state) == true)
            {
                CurrentState = state;
                OnStateChangedEvent?.Invoke();
            }
            else
            {
                Debug.LogError($"Can`t find state for {stateType} in {gameObject.name}.");
            }
        }
    }
   
}