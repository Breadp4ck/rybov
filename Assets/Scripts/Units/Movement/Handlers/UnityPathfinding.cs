using UnityEngine;
using UnityEngine.AI;

namespace Units.Movement
{
    [RequireComponent(typeof(NavMeshAgent))]
    public class UnityPathfinding : MonoBehaviour, IMovementHandler
    {
        public Vector3 Position => _managedTransform.position;
        [SerializeField] private Transform _managedTransform;

        [SerializeField] private NavMeshAgent _agent;

        private Transform _target;
        
        private void Awake()
        {
            _agent.updateRotation = false;
            _agent.updateUpAxis = false;
        }
        
        public void Init()
        {
            _agent.enabled = true;
            _agent.isStopped = false;
        }

        private void FixedUpdate()
        {
            if (_target == null)
            {
                return;
            }
            
            SetDestination(_target.position);
        }

        public void Stop()
        {
            if (_agent.enabled == false)
            {
                return;
            }
            
            _agent.isStopped = true;
            _agent.enabled = false;
        }

        public void SetSpeed(float speed)
        {
            _agent.speed = speed;
        }

        public void SetTarget(Transform target)
        {
            _target = target;
        }

        public void SetDestination(Vector3 destination)
        {
            if (_agent.enabled == false)
            {
                return;
            }
            
            _agent.SetDestination(destination);
        }
    }
}