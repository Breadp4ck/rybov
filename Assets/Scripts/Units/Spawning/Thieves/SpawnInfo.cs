using UnityEngine;

namespace Units.Spawning
{
    [System.Serializable]
    public struct SpawnInfo
    {
        public GameObject FishThiefPrefab => _fishThiefPrefab;
        [SerializeField] private GameObject _fishThiefPrefab;

        public uint SpawnCost => _spawnCost;
        [Range(1, 5)]
        [SerializeField] private uint _spawnCost;
        
        public float SpawnChance => _spawnChance;
        [Range(0f, 1f)]
        [SerializeField] private float _spawnChance;
    }
}