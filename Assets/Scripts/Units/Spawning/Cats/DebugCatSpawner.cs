using UnityEngine;

namespace Units.Spawning
{
    public class DebugCatSpawner : MonoBehaviour, ISpawner
    {
        [SerializeField] private GameObject _catPrefab;
        [SerializeField] private Camera _camera;

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.C) == false)
            {
                return;
            }
            
            Vector3 worldPosition = _camera.ScreenToWorldPoint(Input.mousePosition);
            worldPosition.z = _camera.nearClipPlane;
            
            Spawn(worldPosition);
        }

        public void Spawn(Vector2 position)
        {
            Instantiate(_catPrefab, position, Quaternion.identity);
        }
    }
}
