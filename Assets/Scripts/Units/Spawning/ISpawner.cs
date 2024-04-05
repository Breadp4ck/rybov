using UnityEngine;

namespace Units.Spawning
{
    public interface ISpawner
    {
        GameObject Spawn(GameObject prefab);
    }
}
