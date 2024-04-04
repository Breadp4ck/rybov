using UnityEngine;

namespace Units.Movement.Shared
{
    public class StunnedState : MovementState
    {
        public StunnedState(StateMachine stateMachine) : base(stateMachine) { }

        public override void Start()
        {
            Debug.Log("StunnedState.Start()");
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
            Vector2 direction = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized;
            
            Debug.Log("KickedOutState.Start()");
            IMovementHandler movementHandler = StateMachine.MovementHandler;
            movementHandler.Init();
            movementHandler.SetSpeed(_speed);
            movementHandler.SetTarget(null);
            movementHandler.SetDestination(direction * 10f); // TODO: Work together w/ Issue #26
        }
    }
}
