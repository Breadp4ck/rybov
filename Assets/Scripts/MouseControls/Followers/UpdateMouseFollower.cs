using UnityEngine;

namespace MouseControls
{
    public class UpdateMouseFollower : MouseFollower
    {
        [SerializeField] private Camera _camera;
        
        private void Update()
        {
            Follow();
        }
        
        protected override void Follow()
        {
            Vector3 mousePosition = _camera.ScreenToWorldPoint(Input.mousePosition);
            transform.position = new Vector3(mousePosition.x, mousePosition.y, 0);
        }
    }
}