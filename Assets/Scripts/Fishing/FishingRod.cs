using System.Collections;
using Fishing.Handlers;
using Fishing.Pool;
using UnityEngine;

namespace Fishing
{
    public class FishingRod : MonoBehaviour
    {
        [SerializeField] private float _radius;
        public float Radius => _radius;
        
        public static FishingRod CurrentCatcher { get; private set; }
        
        [SerializeField] private FishLake _fishLake;

        private Vector3 _initialPosition;
        public Vector3 InitialPosition => _initialPosition;
        
        private float _maxDeviation = 0.1f;
        private float _deviationFrequency = 10f;

        private float _nextDeviationTime = 0f;
        private Vector3 _deviationVector = Vector3.zero;
        
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.magenta;
            Gizmos.DrawWireSphere(transform.position, _radius);
        }
        
        private void Awake()
        {
            _initialPosition = transform.position;
        }
        
        public void StartCatching()
        {
            if (_fishLake.IsCatching == true)
            {
                return;
            }

            CurrentCatcher = this;
            _fishLake.StartCatching();
        }

        public void InterruptCatching()
        {
            if (_fishLake.IsCatching == false)
            {
                return;
            }

            CurrentCatcher = null;
            _fishLake.StopCatching(CatchHandler.CatchResult.Fail);
        }

        public void ReturnToInitialPosition() => transform.position = _initialPosition;

        public IEnumerator FollowDragTransform(Transform followTransform)
        {
            Vector2 offset = Vector2.one;

            while (true)
            {
                if (Time.time >= _nextDeviationTime)
                {
                    _deviationVector = Random.insideUnitSphere * _maxDeviation * offset / 100f ;
        
                    _nextDeviationTime = Time.time + 1f / _deviationFrequency;
                }
                
                Vector2 newPosition = Vector2.Lerp(transform.position, followTransform.position, 1f / offset.magnitude / 500f);
                offset = (newPosition - (Vector2)_initialPosition) + (Vector2)_deviationVector;
                transform.position = (Vector2)_initialPosition + Vector2.ClampMagnitude(offset, _radius);
                
                yield return null;
            }
            // ReSharper disable once IteratorNeverReturns
        }
    }
}
