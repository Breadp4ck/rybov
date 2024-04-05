using System;
using UnityEngine;

namespace Units.Movement.Handlers
{
    public class SimpleTranslate : MonoBehaviour, IMovementHandler
    {
        public Vector3 Position => ManagedTransform.position;
        public Transform ManagedTransform { get; set; }
        
        private float _speed;

        private Transform _target;
        private Vector2 _destination;
        
        public void Init() { }

        private void Update()
        {
            Vector2 direction;
            if (_target != null)
            {
                direction = (_target.position - ManagedTransform.position).normalized;
            }
            else
            {
                direction = (_destination - (Vector2)ManagedTransform.position).normalized;
            }
            
            ManagedTransform.Translate(direction * _speed * Time.deltaTime);
        }

        public void Stop()
        {
            _target = null;
        }

        public void SetSpeed(float speed)
        {
            _speed = speed;
        }

        public void SetTarget(Transform target)
        {
            _target = target;
        }

        public void SetDestination(Vector2 destination)
        {
            _destination = destination;
        }
    }
}