using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Fishing.Pool;
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
        private Game Game = Game.Instance;
        
        public override StateType Type => StateType.Start;

        private readonly List<FishLake> _fishLakes;

        public StartState(List<FishLake> fishLakes)
        {
            _fishLakes = fishLakes;
        }

        public override void Start()
        {
            _fishLakes.ForEach(x => x.StartCatchingEvent += OnStartCatching);
        }

        public override void Stop()
        {
            _fishLakes.ForEach(x => x.StartCatchingEvent -= OnStartCatching);
        }

        private void OnStartCatching()
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
                Game.Instance.SetRemainingAssaultTime(_assaultDurationSeconds - timePassedSeconds);
                
                await Task.Delay(delayMs, _cancellationToken);
            }

            await Task.Yield();
            
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

        private CancellationToken _cancellationToken;
        
        public override async void Start()
        {
            SpawnersHandler.Instance.StopSpawning();

            await Task.Yield();
            
            const int pollingDelayMs = 200;
            while (SpawnersHandler.Instance.SpawnedThieves.Any(x => x != null))
            {
                await Task.Delay(pollingDelayMs, _cancellationToken);
            }
            
            Game.Instance.ChangeState(StateType.Finish);
        }
        
        public override void Stop()
        {
            _cancellationToken = new CancellationToken(true);
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