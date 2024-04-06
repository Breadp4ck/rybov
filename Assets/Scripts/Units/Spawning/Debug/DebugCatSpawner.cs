using UnityEngine;

namespace Units.Spawning
{
    public class DebugCatSpawner : MonoBehaviour, ISpawner
    {
        [SerializeField] private GameObject _catPrefab;
        [SerializeField] private Camera _camera;

        private Vector3 _mouseWorldPosition;
        
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.C) == false)
            {
                return;
            }
            
            _mouseWorldPosition = _camera.ScreenToWorldPoint(Input.mousePosition);
            _mouseWorldPosition.z = _camera.nearClipPlane;
            
            Spawn(_catPrefab);
        }

        public GameObject Spawn(GameObject prefab)
        {
            GameObject go = Instantiate(prefab, _mouseWorldPosition, Quaternion.identity);
            SpawnersHandler.Instance.SpawnedThieves.Add(go);

            return go;
        }
    }
}
