using System;
using System.Collections;
using Units.Destroying;
using Units.Dragging;
using Units.Movement.Fish;
using Units.Spawning;
using UnityEngine;

namespace Units
{
    /// <summary>
    /// Fish that registers in FishPool.cs
    /// </summary>
    public class StealableFish : MonoBehaviour, IOutOfBorderInteractable, IDraggable
    {
        public enum Type : byte
        {
            // TODO: Rename
            Fish1,
            Fish2,
            Fish3,
            Fish4,
        }
        
        public event Action<StealableFish> DragStoppedEvent;
        
        /// <summary>
        /// The one who stole and carries this fish at the moment.
        /// </summary>
        public IFishThief Thief;

        public Type FishType => _type;
        [SerializeField] private Type _type;
        
        public FishMovementStateMachine StateMachine => _stateMachine;
        [SerializeField] private FishMovementStateMachine _stateMachine;

        [Header("Dragging")] 
        [SerializeField] private float _lerpSpeed;
        [SerializeField] private float _minDistanceForRotation;
        [SerializeField] private float _rotationSpeed;

        [Header("Sprite Scaling")] 
        [SerializeField] private SpriteRenderer _sprite;

        [SerializeField] private Vector2 _minScaleOnDrag;
        [SerializeField] private Vector2 _maxScaleOnDrag;
        private Vector2 _defaultScale;

        private IEnumerator _followDragTransformRoutine;

        private void Awake()
        {
            _defaultScale = _sprite.transform.localScale;
        }

        #region IOutOfBorderInteractable

        public void OnSteal(IFishThief thief)
        {
            Thief = thief;
            _stateMachine.TryChangeState<CarriedState>();
        }
        
        public void OnDrop()
        {
            Thief = null;
            _stateMachine.TryChangeState<FidgetingCooldownState>();
        }

        public void OnOutOfBorder()
        {
            // Let thief handle out of border.
            if (Thief != null)
            {
                return;
            }
            
            FishPool.OutOfBorder(this);
            Destroy(gameObject);
        }

        #endregion

        #region IDraggable

        public void StartDrag(Transform followTransform)
        {
            if (Thief != null)
            {
                Debug.Log("Fish is already stolen. Can't drag it.");
                return;
            }

            _stateMachine.TryChangeState<CarriedState>();

            if (_followDragTransformRoutine != null)
            {
                StopCoroutine(_followDragTransformRoutine);
            }

            _followDragTransformRoutine = FollowDragTransform(followTransform);
            StartCoroutine(_followDragTransformRoutine);
        }

        public void StopDrag()
        {
            _sprite.transform.localScale = _defaultScale;
            
            if (_followDragTransformRoutine != null)
            {
                StopCoroutine(_followDragTransformRoutine);
            }
            
            DragStoppedEvent?.Invoke(this);
            
            if (Thief != null)
            {
                return;
            }
            
            _stateMachine.TryChangeState<FidgetingState>();
        }
        
        private IEnumerator FollowDragTransform(Transform followTransform)
        {
            Vector2 previousPosition = transform.position;

            while (true)
            {
                // Используем Lerp для плавного перемещения объекта к позиции followTransform.
                Vector2 newPosition = Vector2.Lerp(transform.position, followTransform.position, Time.deltaTime * _lerpSpeed);
                transform.position = newPosition;

                // Вычисляем угол между старым и новым направлением движения только если пройдено достаточное расстояние.
                if (Vector2.Distance(newPosition, previousPosition) >= _minDistanceForRotation)
                {
                    Vector2 oldDirection = transform.up;
                    Vector2 newDirection = newPosition - previousPosition;
                    float angle = Vector2.SignedAngle(oldDirection, newDirection);

                    // Ограничиваем угол поворота относительно горизонта.
                    angle = Mathf.Clamp(angle, -45, 45);

                    // Поворачиваем объект на вычисленный угол.
                    Quaternion currentRotation = transform.rotation;
                    Quaternion targetRotation = Quaternion.Euler(0, 0, angle);
                    transform.rotation = Quaternion.Lerp(currentRotation, targetRotation, Time.deltaTime * _rotationSpeed);
                }

                // Вычисляем пройденное расстояние за кадр.
                float distanceTravelled = Vector2.Distance(newPosition, previousPosition);

                // Вычисляем процентное соотношение пройденного расстояния к максимально возможному расстоянию за кадр.
                float lerpFactor = distanceTravelled / (_lerpSpeed * Time.deltaTime);

                // Интерполируем между _minScaleOnDrag и _maxScaleOnDrag на основе lerpFactor.
                _sprite.transform.localScale = Vector2.Lerp(_minScaleOnDrag, _maxScaleOnDrag, lerpFactor);

                previousPosition = newPosition;

                yield return null;
            }
            // ReSharper disable once IteratorNeverReturns
        }
        
        #endregion
    }
}