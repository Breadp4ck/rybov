using UnityEngine;

namespace MouseControls
{
    public class MouseFollower : MonoBehaviour
    {
        [SerializeField] private Camera _camera;
    
        private void FixedUpdate()
        {
            Vector3 mousePosition = _camera.ScreenToWorldPoint(Input.mousePosition);
            transform.position = new Vector3(mousePosition.x, mousePosition.y, 0);
        }
    }   
}
