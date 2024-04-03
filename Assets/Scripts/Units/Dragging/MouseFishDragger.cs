using System.Linq;
using Inputs;
using UnityEngine;
using Zenject;

namespace Units.Dragging
{
    [RequireComponent(typeof(Collider2D), typeof(Rigidbody2D))]
    public class MouseFishDragger : MonoBehaviour
    {
        [SerializeField] private Camera _camera;

        [Header("Range")] [Range(0.1f, 2f)] [SerializeField]
        private float _radius;
        
        private IInputSystem _inputSystem;

        private IDraggable _draggable;

        [Inject]
        private void Construct(IInputSystem inputSystem)
        {
            _inputSystem = inputSystem;
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(transform.position, _radius);
        }

        private void Update()
        {
            if (_inputSystem.IsActionDown(InputAction.RightClick) == true)
            {
                IDraggable draggable = GetOverlappingDraggable();

                if (draggable == null)
                {
                    return;
                }
                
                draggable.StartDrag(transform);
                _draggable = draggable;
            }

            if (_inputSystem.IsActionUp(InputAction.RightClick) == true)
            {
                if (_draggable != null)
                {
                    _draggable.StopDrag();
                }
                
                _draggable = null;
            }
        }

        private void FixedUpdate()
        {
            transform.position = _camera.ScreenToWorldPoint(Input.mousePosition);
        }
        
        private IDraggable GetOverlappingDraggable()
        {
            Collider2D[] overlappedColliders = new Collider2D[10];
            int count = Physics2D.OverlapCircleNonAlloc(transform.position, _radius, overlappedColliders);

            if (count == 0)
            {
                return null;
            }
            
            overlappedColliders = overlappedColliders.Where(x => x != null).ToArray();
            
            foreach (Collider2D overlapped in overlappedColliders)
            {
                if (overlapped.TryGetComponent(out IDraggable draggable) == false)
                {
                    continue;
                }

                return draggable;
            }

            return null;
        }
    }
}