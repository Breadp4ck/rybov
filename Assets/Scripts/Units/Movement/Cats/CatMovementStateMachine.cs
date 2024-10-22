using System;
using System.Collections.Generic;
using GlobalStates.Game;
using Units.Movement.Handlers;
using Units.Movement.Shared;
using Units.Spawning;
using UnityEngine;

namespace Units.Movement.Cat
{
    #region States
    
    public class WaitForFishSpawn : MovementState
    {
        public WaitForFishSpawn(StateMachine stateMachine) : base(stateMachine) { }
        
        public override void Start()
        {
            if (FishPool.FreeFishes.Count > 0)
            {
                StateMachine.TryChangeState<ChaseForFishState>();
            }
            
            FishPool.FishCaughtEvent += OnFishAdded;
            FishPool.FishDroppedEvent += OnFishAdded;
        }

        public override void TryChangeState(float deltaSeconds)
        {
            if (Game.Instance.CurrentState.Type != StateType.Assault)
            {
                StateMachine.TryChangeState<Runaway>();
            }
        }

        public override void Stop()
        {
            FishPool.FishCaughtEvent -= OnFishAdded;
            FishPool.FishDroppedEvent -= OnFishAdded;
        }

        private void OnFishAdded(StealableFish fish)
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
        
        private IMovementHandler MovementHandler => StateMachine.MovementHandler;
        
        public ChaseForFishState(StateMachine stateMachine, IFishThief thief, float speed, float takeFishRange) : base(stateMachine)
        {
            _thief = thief;
            
            _speed = speed;
            _takeFishRange = takeFishRange;
        }

        public override void Start()
        {
            SetTargetFish();

            MovementHandler.SetSpeed(_speed);
            MovementHandler.Init();
            
            FishPool.FishStolenEvent += OnFishStolen;
        }

        public override void TryChangeState(float deltaSeconds)
        {
            if (Game.Instance.CurrentState.Type != StateType.Assault)
            {
                StateMachine.TryChangeState<Runaway>();
            }
            
            if (_targetFish == null)
            {
                SetTargetFish();
                return;
            }

            if (Vector2.Distance(_targetFish.transform.position, MovementHandler.Position) > _takeFishRange)
            {
                return;
            }

            FishPool.TryStealFish(_targetFish, _thief);
            StateMachine.TryChangeState<Runaway>();
        }

        public override void Stop()
        {
            MovementHandler.Stop();
            
            FishPool.FishStolenEvent -= OnFishStolen;
        }

        private void OnFishStolen(StealableFish fish)
        {
            // If fish that we`re chasing is taken by someone else.
            if (_targetFish == null || _targetFish.Thief != _thief)
            {
                SetTargetFish();
            }
        }
        
        private void SetTargetFish()
        {
            try
            {
                _targetFish = FishPool.GetClosestTo(MovementHandler.Position);

                if (_targetFish == null)
                {
                    StateMachine.TryChangeState<WaitForFishSpawn>();
                    return;
                }
            
                MovementHandler.SetTarget(_targetFish.transform);
            }
            catch (Exception)
            {
                // ignored.
            }
        }
    }

    /// <summary>
    /// Running away the camera border with the fish in hands.
    /// </summary>
    public class Runaway : MovementState
    {
        private readonly float _speed;
        
        private IMovementHandler MovementHandler => StateMachine.MovementHandler;
        
        public Runaway(StateMachine stateMachine, float speed) : base(stateMachine)
        {
            _speed = speed;
        }

        public override void Start()
        {
            MovementHandler.Init();
            MovementHandler.SetSpeed(_speed);
            MovementHandler.SetTarget(SpawnersHandler.Instance.GetRandomSpawner().transform);
        }

        public override void Stop()
        {
            MovementHandler.Stop();
        }
    }
    
    #endregion
    
    public class CatMovementStateMachine : StateMachine
    {
        public override IMovementHandler MovementHandler { get; set; }
        protected override IEnumerable<MovementState> States { get; set; }
        
        [SerializeField] private Units.Cat _cat;
        
        [Header("Pilfering")]
        [SerializeField] private float _pilferSpeed;
        [SerializeField] private float _takeFishRange;
        
        [Header("Runaway With Fish")]
        [SerializeField] private float _runawaySpeed;
        
        [Header("Kick Out")]
        [SerializeField] private float _kickOutSpeed;

        private void Awake()
        {
            MovementHandler = GetComponent<IMovementHandler>();
            
            // Init states.
            States = new MovementState[]
            {
                new WaitForFishSpawn(this),
                new ChaseForFishState(this, _cat, _pilferSpeed, _takeFishRange),
                new Runaway(this, _runawaySpeed),
                new StunnedState(this),
                new KickedOutState(this, _kickOutSpeed)
            };

            if (Game.Instance.CurrentState.Type != StateType.Assault)
            {
                TryChangeState<Runaway>();
                return;
            }
            
            TryChangeState<ChaseForFishState>();
        }

        private void Update()
        {
            CurrentState?.Update(Time.deltaTime);
            CurrentState?.TryChangeState(Time.deltaTime);
        }

        private void FixedUpdate()
        {
            CurrentState?.FixedUpdate(Time.fixedDeltaTime);
        }
    }
   
}