using UnityEngine;

public enum MovementStateType : byte
{
    Idle,
    Walk,
    Dash,
    Run,
}

public abstract class MovementState
{
    public abstract MovementStateType State { get; }


    protected readonly IStateMachine StateMachine;
    protected IInputSystem InputSystem => StateMachine.InputSystem;
    
    protected Transform Transform => StateMachine.ManagedTransform;

    protected MovementState(IStateMachine stateMachine)
    {
        StateMachine = stateMachine;
    }

    public virtual void Start() { }
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
        Vector2 movementDirection = InputSystem.GetMovementDirection();
        if (movementDirection.SqrMagnitude() > 0.0f)
        {
            SetState(MovementStateType.Walk);
        }

        if (InputSystem.IsActionPressed(InputAction.Dash))
        {
            SetState(MovementStateType.Dash);
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
        Vector2 movementDirection = InputSystem.GetMovementDirection();
        if (movementDirection.SqrMagnitude() == 0.0f)
        {
            SetState(MovementStateType.Idle);
        }

        if (InputSystem.IsActionPressed(InputAction.Run) == true)
        {
            SetState(MovementStateType.Run);
        }
        
        if (InputSystem.IsActionPressed(InputAction.Dash) == true)
        {
            SetState(MovementStateType.Dash);
        }
    }

    public override void FixedUpdate()
    {
        Vector2 movementDirection = InputSystem.GetMovementDirection();
        Transform.Translate(movementDirection * _speed * Time.fixedDeltaTime);
    }
}

public class RunState : MovementState
{
    public override MovementStateType State => MovementStateType.Run;
    
    private readonly float _speed;

    public RunState(IStateMachine stateMachine, float speed) : base(stateMachine)
    {
        _speed = speed;
    }

    public override void Update()
    {
        Vector2 movementDirection = InputSystem.GetMovementDirection();
        if (movementDirection.SqrMagnitude() == 0.0f)
        {
            SetState(MovementStateType.Idle);
        }
        
        if (StateMachine.InputSystem.IsActionPressed(InputAction.Dash))
        {
            SetState(MovementStateType.Dash);
        }
    }

    public override void FixedUpdate()
    {
        Vector2 movementDirection = InputSystem.GetMovementDirection();
        Transform.Translate(movementDirection * _speed * Time.fixedDeltaTime);
    }
}

public class DashState : MovementState
{
    public struct DashInfo
    {
        public readonly float Speed;
        public readonly float DurationSeconds;

        public DashInfo(float speed, float durationSeconds)
        {
            Speed = speed;
            DurationSeconds = durationSeconds;
        }
    }
    
    public DashState(IStateMachine stateMachine, DashInfo dashInfo) : base(stateMachine)
    {
        _dashInfo = dashInfo;
    }

    public override MovementStateType State => MovementStateType.Dash;

    private readonly DashInfo _dashInfo;
    private Vector2 _direction;

    private float _durationLeftSeconds;
    
    public override void Start()
    {
        _direction = InputSystem.LookDirection;
        _durationLeftSeconds = _dashInfo.DurationSeconds;
    }

    public override void Update()
    {
        _durationLeftSeconds -= Time.deltaTime;
        if (_durationLeftSeconds <= 0.0f)
        {
            SetState(MovementStateType.Idle);
        }
    }

    public override void FixedUpdate()
    {
        Transform.Translate(_direction * _dashInfo.Speed * Time.fixedDeltaTime);
    }
}