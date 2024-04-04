using UnityEngine;

namespace Units.Spawning
{
    public interface ISpawner
    {
        void Spawn(GameObject prefab);
    }
}
