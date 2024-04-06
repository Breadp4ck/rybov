using System.Collections;
using Fishing.Handlers;
using UnityEngine;

namespace Fishing
{
    [RequireComponent(typeof(IFishLake))]
    public class FishingRod : MonoBehaviour
    {
        [SerializeField] private float _radius;
        public float Radius => _radius;
        
        public static FishingRod CurrentCatcher { get; private set; }
        
        private IFishLake _fishLake;

        private Vector3 _initialPosition;
        public Vector3 InitialPosition => _initialPosition;
        
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.magenta;
            Gizmos.DrawWireSphere(transform.position, _radius);
        }
        
        private void Awake()
        {
            _fishLake = GetComponentInParent<IFishLake>();
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
            const float lerpSpeed = 0.1f; // Скорость интерполяции. Можно настроить по своему усмотрению.

            while (true)
            {
                Vector2 newPosition = Vector2.Lerp(transform.position, followTransform.position, lerpSpeed);
                Vector2 offset = newPosition - (Vector2)_initialPosition;
                transform.position = (Vector2)_initialPosition + Vector2.ClampMagnitude(offset, _radius);
                
                yield return null;
            }
            // ReSharper disable once IteratorNeverReturns
        }
    }
}
