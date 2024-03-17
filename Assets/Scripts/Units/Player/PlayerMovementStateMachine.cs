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
    
    public event Action OnStateChangedEvent;

    public Transform ManagedTransform => _managedTransform;
    [SerializeField] private Transform _managedTransform;

    [Header("Walk")]
    [SerializeField] private float _walkSpeed;
    
    [Header("Walk")]
    [SerializeField] private float _runSpeed;
    
    [Header("Dash")]
    [SerializeField] private float _dashSpeed;
    [SerializeField] private float _dashDurationSeconds;

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
            new WalkState(this, _walkSpeed),
            new RunState(this, _runSpeed),
            new DashState(this, new DashState.DashInfo(_dashSpeed, _dashDurationSeconds)),
        };

        TrySetState(MovementStateType.Idle);
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
        CurrentState.Start();
        
        OnStateChangedEvent?.Invoke();
        
        return true;
    }
}
