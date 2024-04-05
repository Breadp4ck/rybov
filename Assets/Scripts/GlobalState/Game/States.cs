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
        
        private CancellationToken _cancellationToken;
        
        public AssaultState(float assaultDurationSeconds)
        {
            _assaultDurationSeconds = assaultDurationSeconds;
        }
        
        public override async void Start()
        {
            float timePassedSeconds = 0;
            const int delayMs = 200;
            while (timePassedSeconds < _assaultDurationSeconds)
            {
                timePassedSeconds += delayMs / 1000f;
                await Task.Delay(delayMs, _cancellationToken);
            }

            if (_cancellationToken.IsCancellationRequested == true)
            {
                return;
            }
            
            Game.Instance.ChangeState(StateType.Fleeing);
        }

        public override void Stop()
        {
            _cancellationToken = new CancellationToken(true);
        }
    }
    
    public class FleeingState : State
    {
        public override StateType Type => StateType.Fleeing;

        public override async void Start()
        {
            SpawnersHandler.Instance.StopSpawning();

            const int pollingDelayMs = 200;
            while (SpawnersHandler.Instance.SpawnedThieves.Any(x => x != null))
            {
                await Task.Delay(pollingDelayMs);
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