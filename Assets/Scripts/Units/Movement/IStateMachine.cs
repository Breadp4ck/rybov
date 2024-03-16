using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IStateMachine
{
    IInputSystem InputSystem { get; }
    
    MovementState CurrentState { get; }
    IEnumerable<MovementState> States { get; }
    
    /// <summary>
    /// Represents the transform of the object that is being managed by the state machine.
    /// </summary>
    Transform ManagedTransform { get; }
    
    bool TrySetState(MovementStateType stateType);
}