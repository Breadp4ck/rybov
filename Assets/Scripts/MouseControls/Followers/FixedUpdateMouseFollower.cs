using System;
using UnityEngine;

namespace MouseControls
{
    public class FixedUpdateMouseFollower : MouseFollower
    {
        [SerializeField] private Camera _camera;
        
        private void FixedUpdate()
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