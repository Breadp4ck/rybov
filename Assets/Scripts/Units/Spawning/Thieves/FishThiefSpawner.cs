using UnityEngine;

namespace Units.Spawning
{
    public class FishThiefSpawner : MonoBehaviour, ISpawner
    {
        public GameObject Spawn(GameObject prefab)
        {
            Debug.Log($"Spawned: {prefab.name}");
            return Instantiate(prefab, transform.position, Quaternion.identity);
        }
    }
}