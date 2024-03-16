using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Zenject;

public class PlayerMovementStateMachine : MonoBehaviour, IStateMachine
{
    public IInputSystem InputSystem { get; private set; }
    
    public MovementState CurrentState { get; private set; }

    public IEnumerable<MovementState> States { get; private set; }

    public Transform ManagedTransform => _managedTransform;
    [SerializeField] private Transform _managedTransform;

    [SerializeField] private float _walkSpeed;

    [Inject]
    private void Construct(IInputSystem inputSystem)
    {
        InputSystem = inputSystem;
    }
    
    private void Start()
    {
        // Init states.
        States = new MovementState[]
        {
            new IdleState(this),
            new WalkState(this, _walkSpeed)
        };
        
        CurrentState = States.FirstOrDefault(x => x.State == MovementStateType.Idle);
    }

    private void Update()
    {
        CurrentState?.Update();
    }

    private void FixedUpdate()
    {
        CurrentState?.FixedUpdate();
    }
    
    public bool TrySetState(MovementStateType stateType)
    {
        MovementState state = States.FirstOrDefault(x => x.State == stateType);
        
        if (state == null)
        {
            return false;
        }
        
        CurrentState = state;
        return true;
    }
}
