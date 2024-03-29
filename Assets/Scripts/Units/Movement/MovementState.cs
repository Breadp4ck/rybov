using Inputs;
using Movement;
using UnityEngine;

namespace Movement
{
    public abstract class MovementState
    {
        protected readonly IStateMachine StateMachine;

        protected Transform Transform => StateMachine.ManagedTransform;

        protected MovementState(IStateMachine stateMachine)
        {
            StateMachine = stateMachine;
        }

        public virtual void Start() { }

        public virtual void Update() { }

        public virtual void TryChangeState() { }
    }
}