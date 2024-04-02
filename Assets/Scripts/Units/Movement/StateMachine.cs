using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Units.Movement
{
    [RequireComponent(typeof(IMovementHandler))]
    public abstract class StateMachine : MonoBehaviour
    {
        public event Action<MovementState> StateChangedEvent;
        
        public IMovementHandler MovementHandler { get; protected set; }

        protected MovementState CurrentState { get; private set; }

        protected IEnumerable<MovementState> States { get; set; }

        public bool TryChangeState<T>() where T : MovementState
        {
            T newState = States.OfType<T>().FirstOrDefault();
            if (newState != null)
            {
                CurrentState?.Stop();
                CurrentState = newState;
                CurrentState.Start();

                StateChangedEvent?.Invoke(newState);
                
                return true;
            }

            Debug.LogError($"State of type {typeof(T)} not found in state machine.");
            return false;
        }
    }
}