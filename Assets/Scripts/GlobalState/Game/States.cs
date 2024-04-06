using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Units.Spawning;

namespace GlobalStates.Game
{
    public abstract class State
    {
        public abstract StateType Type { get; }
        
        public abstract void Start();
        public virtual void Stop() { }

        public virtual void Update(float deltaSeconds) { }
    }
    
    public class StartState : State
    {
        public override StateType Type => StateType.Start;

        public override void Start()
        {
            Game.Instance.ChangeState(StateType.Assault);
        }
    }
    
    public class AssaultState : State
    {
        public override StateType Type => StateType.Assault;
        
        private readonly float _assaultDurationSeconds;
        private float _timePassedSeconds;
        
        public AssaultState(float assaultDurationSeconds)
        {
            _assaultDurationSeconds = assaultDurationSeconds;
        }
        
        public override void Start()
        {
            _timePassedSeconds = 0;
        }

        public override void Update(float deltaSeconds)
        {
            if (_timePassedSeconds < _assaultDurationSeconds)
            {
                _timePassedSeconds += deltaSeconds;
                return;
            }
            
            Game.Instance.ChangeState(StateType.Fleeing);
        }
    }
    
    public class FleeingState : State
    {
        public override StateType Type => StateType.Fleeing;

        public override void Start()
        {
            SpawnersHandler.Instance.StopSpawning();
        }

        public override void Update(float deltaSeconds)
        {
            if (SpawnersHandler.Instance == null)
            {
                return;
            }
            
            if (SpawnersHandler.Instance.SpawnedThieves.Any(x => x != null))
            {
                return;
            }
            
            Game.Instance.ChangeState(StateType.Finish);
        }
    }
    
    public class FinishState : State
    {
        public override StateType Type => StateType.Finish;

        public override void Start()
        {
            
        }
    }
}