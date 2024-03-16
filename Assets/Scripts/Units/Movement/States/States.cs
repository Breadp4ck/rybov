using UnityEngine;

public enum MovementStateType : byte
{
    Idle,
    Walk,
}

public abstract class MovementState
{
    public abstract MovementStateType State { get; }

    protected Transform Transform => StateMachine.ManagedTransform;

    protected readonly IStateMachine StateMachine;

    protected MovementState(IStateMachine stateMachine)
    {
        StateMachine = stateMachine;
    }

    public virtual void Update() { }
    public virtual void FixedUpdate() { }

    protected void SetState(MovementStateType stateType)
    {
        StateMachine.TrySetState(stateType);
    }
}

public class IdleState : MovementState
{
    public override MovementStateType State => MovementStateType.Idle;

    public IdleState(IStateMachine stateMachine) : base(stateMachine) { }

    public override void Update()
    {
        Vector2 movementDirection = StateMachine.InputSystem.GetMovementDirection();
        if (movementDirection.SqrMagnitude() > 0.0f)
        {
            SetState(MovementStateType.Walk);
        }
    }
}

public class WalkState : MovementState
{
    public override MovementStateType State => MovementStateType.Walk;
    
    private readonly float _speed;

    public WalkState(IStateMachine stateMachine, float speed) : base(stateMachine)
    {
        _speed = speed;
    }

    public override void Update()
    {
        Vector2 movementDirection = StateMachine.InputSystem.GetMovementDirection();
        if (movementDirection.SqrMagnitude() == 0.0f)
        {
            SetState(MovementStateType.Idle);
        }
    }

    public override void FixedUpdate()
    {
        Vector2 movementDirection = StateMachine.InputSystem.GetMovementDirection();
        Transform.Translate(movementDirection * _speed * Time.fixedDeltaTime);
    }
}