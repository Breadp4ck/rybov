using System.Collections.Generic;
using UnityEngine;

namespace Units.Movement
{
    public interface IStateMachine
    {
        MovementState CurrentState { get; }
        
        IEnumerable<MovementState> States { get; }

        /// <summary>
        /// Represents the transform of the object that is being managed by the state machine.
        /// </summary>
        Transform ManagedTransform { get; }

        bool TryChangeState<T>() where T : MovementState;
    }
}