using Units.Spawning;
using UnityEngine;

namespace Fishing.Fish
{
    public class FishSpawner : MonoBehaviour, ISpawner
    {
        public void Spawn(GameObject prefab)
        {
            Debug.Log($"Spawned: {prefab.name}");
            Instantiate(prefab, transform.position, Quaternion.identity);
        }
    }
}