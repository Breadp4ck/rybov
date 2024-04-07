using System;
using UnityEngine;

namespace Units.Destroying
{
    [RequireComponent(typeof(BoxCollider2D))]
    public class GameFieldBorder : MonoBehaviour
    {
        [SerializeField] private Camera _camera;

        [SerializeField] private Vector2 _offset;
        
        private BoxCollider2D _collider;
        
        private void Awake()
        {
            _collider = GetComponent<BoxCollider2D>();
        }

        private void Start()
        {
            SetupBorders();
        }

        private void SetupBorders()
        {
            Vector2 topRight = _camera.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, _camera.transform.position.z));
            Vector2 bottomLeft = _camera.ScreenToWorldPoint(new Vector3(0, 0, _camera.transform.position.z));

            // Добавляем смещение.
            topRight += _offset;
            bottomLeft -= _offset;

            // Устанавливаем позицию и размер BoxCollider2D.
            _collider.size = topRight - bottomLeft;
            _collider.offset = (topRight + bottomLeft) / 2;
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (other.TryGetComponent(out IOutOfBorderInteractable interactable) == false)
            {
                return;
            }

            interactable.OnOutOfBorder();
        }
    }
}