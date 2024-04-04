using UnityEngine;

namespace Units.Spawning
{
    public class FishThiefSpawner : MonoBehaviour, ISpawner
    {
        public void Spawn(GameObject prefab)
        {
            Debug.Log($"Spawned: {prefab.name}");
            Instantiate(prefab, transform.position, Quaternion.identity);
        }
    }
}