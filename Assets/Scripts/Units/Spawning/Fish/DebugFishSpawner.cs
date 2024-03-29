using UnityEngine;

namespace Units.Spawning
{
    public class DebugFishSpawner : MonoBehaviour, ISpawner
    {
        [SerializeField] private StealableFish _stealableFishPrefab;
        [SerializeField] private Camera _camera;
        
        private void Update()
        {
            if (Input.GetMouseButtonDown(1) == false)
            {
                return;
            }
            
            Vector3 worldPosition = _camera.ScreenToWorldPoint(Input.mousePosition);
            worldPosition.z = _camera.nearClipPlane;
            
            Spawn(worldPosition);
        }

        public void Spawn(Vector2 position)
        {
            StealableFish stealableFish = Instantiate(_stealableFishPrefab, position, Quaternion.identity);
            FishPool.CatchFish(stealableFish);
        }
    }
}