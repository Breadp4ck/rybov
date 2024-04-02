using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Units.Movement.Fish
{
    #region States

    public class FidgetingCooldownState : MovementState
    {
        private readonly TimeSpan _cooldownTime;
        private TimeSpan _currentTime;
        
        public FidgetingCooldownState(IStateMachine stateMachine, TimeSpan cooldownTime) : base(stateMachine)
        {
            _cooldownTime = cooldownTime;
        }
        
        public override void Start()
        {
            _currentTime = TimeSpan.Zero;
        }

        public override void Update(float deltaSeconds)
        {
            if (_currentTime < _cooldownTime)
            {
                _currentTime += TimeSpan.FromSeconds(deltaSeconds);
                return;
            }

            StateMachine.TryChangeState<FidgetingState>();
        }
    }

    public class FidgetingState : MovementState
    {
        public struct Info
        {
            public readonly float MinRange;
            public readonly float MaxRange;
            
            public readonly float MinSpeed;
            public readonly float MaxSpeed;

            public Info(float minRange, float maxRange, float minSpeed, float maxSpeed)
            {
                MinRange = minRange;
                MaxRange = maxRange;
                MinSpeed = minSpeed;
                MaxSpeed = maxSpeed;
            }
        }

        private readonly Info _info;

        /// <summary>
        /// Time the fish will fidget.
        /// Can be calculated as (FidgetRange / Speed). 
        /// </summary>
        private TimeSpan _fidgetTime;
        private TimeSpan _currentTime;
        
        private Vector2 _direction;
        private float _distance;
        private float _speed;
        
        public FidgetingState(IStateMachine stateMachine, Info info) : base(stateMachine)
        {
            _info = info;
        }

        public override void Start()
        {
            _currentTime = TimeSpan.Zero;
            
            _direction = new Vector2(UnityEngine.Random.Range(-1f, 1f), UnityEngine.Random.Range(-1f, 1f)).normalized;
            _distance = UnityEngine.Random.Range(_info.MinRange, _info.MaxRange);
            _speed = UnityEngine.Random.Range(_info.MinSpeed, _info.MaxSpeed);
            
            _fidgetTime = TimeSpan.FromSeconds(_distance / _speed);
        }

        public override void Update(float deltaSeconds)
        {
            ManagedTransform.Translate(_direction * (_distance * deltaSeconds));
        }

        public override void TryChangeState(float deltaSeconds)
        {
            if (_currentTime < _fidgetTime)
            {
                _currentTime += TimeSpan.FromSeconds(deltaSeconds);
                return;
            }
            
            StateMachine.TryChangeState<FidgetingCooldownState>();
        }
    }

    /// <summary>
    /// State when the fish is being carried by the thieve.
    /// </summary>
    public class CarriedState : MovementState
    {
        public CarriedState(IStateMachine stateMachine) : base(stateMachine) { }
    }

    #endregion

    public class FishMovementStateMachine : MonoBehaviour, IStateMachine
    {
        public MovementState CurrentState { get; private set; }
        public IEnumerable<MovementState> States { get; private set; }

        public event Action StateChangedEvent;
        
        public Transform ManagedTransform => _managedTransform;
        [SerializeField] private Transform _managedTransform;
        
        [Header("Fidgeting")]
        [SerializeField] private float _cooldownSeconds;
        
        [SerializeField] private float _minFidgetRange;
        [SerializeField] private float _maxFidgetRange;
        
        [SerializeField] private float _minFidgetSpeed;
        [SerializeField] private float _maxFidgetSpeed;
        
        private void Start()
        {
            // Init states.
            States = new MovementState[]
            {
                new FidgetingCooldownState(this, TimeSpan.FromSeconds(_cooldownSeconds)),
                new FidgetingState(this, new FidgetingState.Info(_minFidgetRange, _maxFidgetRange, _minFidgetSpeed, _maxFidgetSpeed)),
                new CarriedState(this)
            };

            TryChangeState<FidgetingCooldownState>();
        }
        
        private void Update()
        {
            CurrentState?.Update(Time.deltaTime);
            CurrentState?.TryChangeState(Time.deltaTime);
        }
        
        public bool TryChangeState<T>() where T : MovementState
        {
            T newState = States.OfType<T>().FirstOrDefault();
            if (newState != null)
            {
                CurrentState = newState;
                CurrentState.Start();
    
                StateChangedEvent?.Invoke();
        
                return true;
            }

            Debug.LogError($"State of type {typeof(T)} not found in state machine.");
            return false;
        }
    }
}