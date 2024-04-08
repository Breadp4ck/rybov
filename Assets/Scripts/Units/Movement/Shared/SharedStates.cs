using Units.Movement.Handlers;
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
            SimpleTranslate movementHandler = StateMachine.gameObject.AddComponent<SimpleTranslate>();

            movementHandler.ManagedTransform = StateMachine.transform;
            
            movementHandler.Init();
            movementHandler.SetSpeed(_speed);
            movementHandler.SetTarget(null);
            
            // Kick out off the screen.
            movementHandler.SetDestination(Random.insideUnitCircle.normalized * 100f);
        }

        public override void Stop()
        {
            StateMachine.TryChangeState<KickedOutState>();
        }
    }
}
