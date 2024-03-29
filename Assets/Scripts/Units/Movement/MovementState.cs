using UnityEngine;

namespace Units.Movement
{
    public abstract class MovementState
    {
        protected readonly IStateMachine StateMachine;

        protected Transform ManagedTransform => StateMachine.ManagedTransform;

        protected MovementState(IStateMachine stateMachine)
        {
            StateMachine = stateMachine;
        }

        public virtual void Start() { }

        public virtual void Update(float deltaSeconds) { }

        public virtual void TryChangeState(float deltaSeconds) { }
        
        public virtual void Stop() { }
    }
}