namespace Units.Movement
{
    public abstract class MovementState
    {
        protected readonly StateMachine StateMachine;
        
        protected MovementState(StateMachine stateMachine)
        {
            StateMachine = stateMachine;
        }

        public virtual void Start() { }

        public virtual void Update(float deltaSeconds) { }
        
        public virtual void FixedUpdate(float deltaSeconds) { }

        public virtual void TryChangeState(float deltaSeconds) { }
        
        public virtual void Stop() { }
    }
}