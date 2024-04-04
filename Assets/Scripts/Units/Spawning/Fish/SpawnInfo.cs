using UnityEngine;

namespace Fishing.Fish
{
    [System.Serializable]
    public struct SpawnInfo
    {
        public GameObject FishPrefab => _fishPrefab;
        [SerializeField] private GameObject _fishPrefab;

        public float SpawnChance => _spawnChance;
        [Range(0f, 1f)] [SerializeField] private float _spawnChance;
    }
}