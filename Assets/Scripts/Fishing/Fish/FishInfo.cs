using Units;
using UnityEngine;

namespace Fishing.Fish
{
    [System.Serializable]
    public record FishInfo
    {
        public StealableFish StealableFish => _stealableFish;
        [SerializeField] private StealableFish _stealableFish;
        
        public float SpawnChance => _spawnChance;
        [Range(0f, 1f)]
        [SerializeField] private float _spawnChance; 

        public float InitialCatchExtent => _initialCatchExtent;
        [Header("Catch Info")]
        [Range(0f, 1f)]
        [SerializeField] private float _initialCatchExtent;

        public float MaxCatchSpeed => _maxCatchSpeed;
        [SerializeField] private float _maxCatchSpeed;
        
        public float MaxLoseSpeed => _maxLoseSpeed;
        [SerializeField] private float _maxLoseSpeed;

        public float FishAggressivityInSeconds => _fishAggressivityInSeconds;
        [SerializeField] private float _fishAggressivityInSeconds;
    }
}