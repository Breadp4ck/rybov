using Units.Movement;
using Units.Spawning;
using UnityEngine;

namespace Units.Movement.Shared
{
    public class StunnedState : MovementState
    {
        public StunnedState(StateMachine stateMachine) : base(stateMachine) { }

        public override void Start()
        {
            StateMachine.MovementHandler.Stop();
        }
    }
    
    public class KickedOutState : MovementState
    {
        private readonly float _speed;
        
        public KickedOutState(StateMachine stateMachine, float speed) : base(stateMachine)
        {
            _speed = speed;
        }

        public override void Start()
        {
            IMovementHandler movementHandler = StateMachine.MovementHandler;
            movementHandler.Init();
            movementHandler.SetSpeed(_speed);
            movementHandler.SetTarget(null);
            movementHandler.SetDestination(SpawnersHandler.Instance.GetRandomSpawner().transform.position);
        }
    }
}
