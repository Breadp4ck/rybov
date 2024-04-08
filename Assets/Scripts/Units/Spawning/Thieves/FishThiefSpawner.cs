using UnityEngine;

namespace Units.Spawning
{
    public class FishThiefSpawner : MonoBehaviour, ISpawner
    {
        public GameObject Spawn(GameObject prefab)
        {
            return Instantiate(prefab, transform.position, Quaternion.identity);
        }
    }
}