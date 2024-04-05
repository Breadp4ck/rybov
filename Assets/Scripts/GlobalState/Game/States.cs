using System.Threading;
using System.Threading.Tasks;
using Units.Spawning;
using UnityEngine;

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
            const int millisecondsDelay = 200;
            while (timePassedSeconds < _assaultDurationSeconds)
            {
                timePassedSeconds += millisecondsDelay / 1000f;
                await Task.Delay(millisecondsDelay, _cancellationToken);
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

        private readonly SpawnersHandler _spawnersHandler;

        public FleeingState(SpawnersHandler spawnersHandler)
        {
            _spawnersHandler = spawnersHandler;
        }

        public override void Start()
        {
            _spawnersHandler.StopSpawning();
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