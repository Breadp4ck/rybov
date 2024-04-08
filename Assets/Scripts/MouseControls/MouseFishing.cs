using System;
using System.Collections;
using System.Linq;
using Fishing;
using Inputs;
using Units.Dragging;
using UnityEngine;
using Zenject;

namespace MouseControls
{
    [RequireComponent(typeof(MouseFollower))]
    public class MouseFishing : MonoBehaviour
    {
        public event Action StartPullFishingRodEvent;
        public event Action StopPullFishingRodEvent;
        
        [Header("Range")] [Range(0.1f, 2f)] [SerializeField]
        private float _radius;
        
        private IInputSystem _inputSystem;
        
        private IEnumerator _followDragTransformRoutine;

        private FishingRod _fishingRod;

        [Inject]
        private void Construct(IInputSystem inputSystem)
        {
            _inputSystem = inputSystem;
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(transform.position, _radius);
        }
        
        private void Update()
        {
            if (_inputSystem.IsActionDown(InputAction.RightClick) == true)
            {
                _fishingRod = GetOverlappingRod();

                if (_fishingRod == null)
                {
                    return;
                }
            
                _fishingRod.StartCatching();
                StartDrag(transform);
            }

            if (_inputSystem.IsActionUp(InputAction.RightClick) == true)
            {
                if (FishingRod.CurrentCatcher == null)
                {
                    return;
                }
                
                FishingRod.CurrentCatcher.ReturnToInitialPosition();
                FishingRod.CurrentCatcher.InterruptCatching();
                StopDrag();
            }
        }
        
        private FishingRod GetOverlappingRod()
        {
            Collider2D[] overlappedColliders = new Collider2D[10];
            int count = Physics2D.OverlapCircleNonAlloc(transform.position, _radius, overlappedColliders);

            if (count == 0)
            {
                return null;
            }
            
            overlappedColliders = overlappedColliders.Where(x => x != null).ToArray();
            
            // Ignore fish rod if there is a draggable object in the area
            if (overlappedColliders.Any(x => x.GetComponent<IDraggable>() != null) == true)
            {
                return null;
            }

            return overlappedColliders.Select(x => x.GetComponent<FishingRod>()).FirstOrDefault(x => x != null);
        }
        
        public void StartDrag(Transform followTransform)
        {
            if (_followDragTransformRoutine != null)
            {
                StopCoroutine(_followDragTransformRoutine);
            }

            _followDragTransformRoutine = _fishingRod.FollowDragTransform(followTransform);
            StartCoroutine(_followDragTransformRoutine);
            
            StartPullFishingRodEvent?.Invoke();
        }

        public void StopDrag()
        {
            if (_followDragTransformRoutine != null)
            {
                StopCoroutine(_followDragTransformRoutine);
            }
            
            StopPullFishingRodEvent?.Invoke();
        }
        
    }   
}