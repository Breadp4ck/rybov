using System;
using System.Collections.Generic;
using System.Linq;
using Units.Spawning;
using UnityEngine;

namespace Units.Movement.Cat
{
    #region States
    
    public class WaitForFishSpawn : MovementState
    {
        public WaitForFishSpawn(IStateMachine stateMachine) : base(stateMachine) { }
        
        public override void Start()
        {
            if (FishPool.FreeFishes.Count > 0)
            {
                StateMachine.TryChangeState<ChaseForFishState>();
            }
            
            FishPool.FishCaughtEvent += OnFishAdded;
            FishPool.FishDroppedEvent += OnFishAdded;
        }

        private void OnFishAdded()
        {
            FishPool.FishCaughtEvent -= OnFishAdded;
            FishPool.FishDroppedEvent -= OnFishAdded;
            
            StateMachine.TryChangeState<ChaseForFishState>();
        }
    }
    
    /// <summary>
    /// Chasing the Fish.
    /// </summary>
    public class ChaseForFishState : MovementState
    {
        private readonly float _speed;
        
        /// <summary>
        /// Range when cat`ll take the fish. And translated to RunawayWithFishState.
        /// </summary>
        private readonly float _takeFishRange;
        
        private StealableFish _targetFish;
        
        private readonly IFishThief _thief;
        
        public ChaseForFishState(IStateMachine stateMachine, IFishThief thief, float speed, float takeFishRange) : base(stateMachine)
        {
            _thief = thief;
            
            _speed = speed;
            _takeFishRange = takeFishRange;
        }

        public override void Start()
        {
            SetTargetFish();
            
            FishPool.FishStolenEvent += OnFishStolen;
        }
        
        public override void Update(float deltaTime)
        {
            if (_targetFish == null)
            {
                SetTargetFish();
                return;
            }
            
            Vector2 direction = (_targetFish.transform.position - StateMachine.ManagedTransform.position).normalized;
            StateMachine.ManagedTransform.Translate(direction * (_speed * deltaTime));
        }

        public override void TryChangeState(float deltaSeconds)
        {
            if (_targetFish == null)
            {
                SetTargetFish();
                return;
            }

            if (Vector2.Distance(_targetFish.transform.position, StateMachine.ManagedTransform.position) > _takeFishRange)
            {
                return;
            }

            FishPool.StealFish(_targetFish, _thief);
            StateMachine.TryChangeState<RunawayWithFishState>();
        }

        public override void Stop()
        {
            FishPool.FishStolenEvent -= OnFishStolen;
        }

        private void OnFishStolen()
        {
            // If fish that we`re chasing is taken by someone else.
            if (_targetFish.Thief != _thief)
            {
                SetTargetFish();
            }
        }
        
        private void SetTargetFish()
        {
            _targetFish = FishPool.GetClosestTo(StateMachine.ManagedTransform.position);

            if (_targetFish == null)
            {
                StateMachine.TryChangeState<WaitForFishSpawn>();
            }
        }
    }

    /// <summary>
    /// Running away the camera border with the fish in hands.
    /// </summary>
    public class RunawayWithFishState : MovementState
    {
        private readonly float _speed;
        
        private Vector2 _direction;
        
        public RunawayWithFishState(IStateMachine stateMachine, float speed) : base(stateMachine)
        {
            _speed = speed;
        }

        public override void Start()
        {
            _direction = new Vector2(UnityEngine.Random.Range(-1f, 1f), UnityEngine.Random.Range(-1f, 1f)).normalized;
        }

        public override void Update(float deltaSeconds)
        {
            StateMachine.ManagedTransform.Translate(_direction * (_speed * deltaSeconds));
        }
    }
    
    #endregion
    
    public class CatMovementStateMachine : MonoBehaviour, IStateMachine
    {
        public MovementState CurrentState { get; private set; }

        public IEnumerable<MovementState> States { get; private set; } = Enumerable.Empty<MovementState>();

        public event Action StateChangedEvent;
    
        [SerializeField] private Units.Cat _cat;
        
        public Transform ManagedTransform => _managedTransform;
        [SerializeField] private Transform _managedTransform;
        
        [Header("Pilfering")]
        [SerializeField] private float _pilferSpeed;
        [SerializeField] private float _takeFishRange;
        
        [Header("Runaway With Fish")]
        [SerializeField] private float _runawaySpeed;
        
        private void Start()
        {
            // Init states.
            States = new MovementState[]
            {
                new WaitForFishSpawn(this),
                new ChaseForFishState(this, _cat, _pilferSpeed, _takeFishRange),
                new RunawayWithFishState(this, _runawaySpeed)
            };

            // Somehow 'Wait' state causes 'Chase' state to be set twice, so it's better to use by default 'Chase' state
            //TryChangeState<WaitForFishSpawn>();
            TryChangeState<ChaseForFishState>();
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
                CurrentState?.Stop();
                CurrentState = newState;
                CurrentState.Start();
    
                StateChangedEvent?.Invoke();
        
                return true;
            }

            Debug.LogError($"State of type {typeof(T)} not found in state machine {typeof(CatMovementStateMachine)}.");
            return false;
        }
    }
   
}