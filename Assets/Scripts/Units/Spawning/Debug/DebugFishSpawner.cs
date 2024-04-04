using UnityEngine;

namespace Units.Spawning
{
    public class DebugFishSpawner : MonoBehaviour, ISpawner
    {
        [SerializeField] private StealableFish _stealableFishPrefab;
        [SerializeField] private Camera _camera;
        
        private Vector3 _mouseWorldPosition;
        
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.F) == false)
            {
                return;
            }
            
            _mouseWorldPosition = _camera.ScreenToWorldPoint(Input.mousePosition);
            _mouseWorldPosition.z = _camera.nearClipPlane;
            
            Spawn(null);
        }

        public void Spawn(GameObject prefab)
        {
            StealableFish stealableFish = Instantiate(_stealableFishPrefab, _mouseWorldPosition, Quaternion.identity);
            FishPool.CatchFish(stealableFish);
        }
    }
}