using UnityEngine;

namespace Units.Movement
{
    public interface IMovementHandler
    {
        public Vector3 Position { get; }

        void Init();
        void Stop();
        
        void SetSpeed(float speed);
        void SetTarget(Transform target);
        void SetDestination(Vector3 destination);
    }
}