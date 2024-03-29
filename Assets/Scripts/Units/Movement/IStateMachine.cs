using System.Collections.Generic;
using Inputs;
using UnityEngine;

namespace Movement
{
    public interface IStateMachine
    {
        MovementState CurrentState { get; }

        /// <summary>
        /// Represents the transform of the object that is being managed by the state machine.
        /// </summary>
        Transform ManagedTransform { get; }
    }
}