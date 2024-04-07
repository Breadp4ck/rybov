using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Fishing.Handlers;
using Fishing.Pool;
using GlobalStates.Game;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Units.Spawning
{
    public class SpawnersHandler : MonoBehaviour
    {
        public static SpawnersHandler Instance { get; private set; }

        public List<GameObject> SpawnedThieves { get; private set; } = new();
        
        [SerializeField] private List<SpawnInfo> _spawnInfo;

        private List<FishThiefSpawner> _spawners;

        private List<FishLake> _fishLakes;

        [SerializeField] private uint _wavesCount;

        [SerializeField] private float _wavesIntervalSeconds;
        [SerializeField] private float _spawnsIntervalSeconds;

        private IEnumerator _spawnThievesWavesRoutine;
        
        private void OnValidate()
        {
            _spawners = FindObjectsOfType<FishThiefSpawner>().ToList();
            _fishLakes = FindObjectsOfType<FishLake>().ToList();
            
            if (Math.Abs(_spawnInfo.Select(x => x .SpawnChance).Sum() - 1) > 0.01f)
            {
                Debug.LogWarning($"Spawn chances in {gameObject} sum are not equal to 1");
            }
        }

        private void OnEnable()
        {
            foreach (FishLake lake in _fishLakes)
            {
                lake.EndCatchingEvent += OnEndCatching;
            }
        }

        private void OnDisable()
        {
            foreach (FishLake lake in _fishLakes)
            {
                lake.EndCatchingEvent -= OnEndCatching;
            }

            StopSpawning();
        }

        private void Awake()
        {
            if (Instance != null)
            {
                Destroy(gameObject);
                return;
            }
            
            Instance = this;
        }

        public void StopSpawning()
        {
            if (_spawnThievesWavesRoutine != null)
            {
                StopCoroutine(_spawnThievesWavesRoutine);
            }
        }
        
        public FishThiefSpawner GetRandomSpawner()
        {
            return _spawners[Random.Range(0, _spawners.Count)];
        }

        private IEnumerator SpawnThievesWaves()
        {
            for (var i = 0; i < _wavesCount; i++)
            {
                uint balance = GetBalance();
                print($"Balance: {balance}");
                while (balance > _spawnInfo.Select(x => x.SpawnCost).Min())
                {
                    SpawnInfo spawnInfo = GetRelativelyRandomSpawnInfoByBalance(balance);
                    GameObject spawnedGo = GetRandomSpawner().Spawn(spawnInfo.FishThiefPrefab);
                    SpawnedThieves.Add(spawnedGo);

                    balance -= spawnInfo.SpawnCost;
                    yield return new WaitForSeconds(_spawnsIntervalSeconds);
                }

                yield return new WaitForSeconds(_wavesIntervalSeconds);
            }
        }
        
        private void OnEndCatching(CatchHandler.CatchResult catchResult)
        {
            if (catchResult != CatchHandler.CatchResult.Success)
            {
                return;
            }

            foreach (FishLake lake in _fishLakes)
            {
                lake.EndCatchingEvent -= OnEndCatching;
            }
            
            if (_spawnThievesWavesRoutine != null)
            {
                StopCoroutine(_spawnThievesWavesRoutine);
            }

            _spawnThievesWavesRoutine = SpawnThievesWaves();
            StartCoroutine(_spawnThievesWavesRoutine);
        }

        /// <summary>
        /// Get balance for the current wave
        /// Balance can be spent on spawning thieves
        /// </summary>
        /// <returns>Balance value</returns>
        private uint GetBalance()
        {
            Game game = Game.Instance;
            
            float timePassed = game.AssaultDurationSeconds - game.RemainingAssaultTimeSeconds;

            return (uint)Mathf.CeilToInt(game.AssaultDurationSeconds * (Mathf.Sqrt(timePassed) / (_wavesCount * _wavesIntervalSeconds)));
        }

        /// <summary>
        /// Get random spawn info from the list of spawn info by SpawnInfo.SpawnChance
        /// </summary>
        /// <returns>Random SpawnInfo relatively to it`s spawn chance</returns>
        private SpawnInfo GetRelativelyRandomSpawnInfoByBalance(uint balance)
        {
            List<SpawnInfo> availableSpawnInfo = _spawnInfo.Where(x => x.SpawnCost <= balance).ToList();
            
            float totalChance = availableSpawnInfo.Sum(info => info.SpawnChance);
            float randomChance = Random.Range(0, totalChance);
            foreach (SpawnInfo info in availableSpawnInfo)
            {
                randomChance -= info.SpawnChance;
                if (randomChance <= 0)
                {
                    return info;
                }
            }
            
            return availableSpawnInfo[0];
        }
    }
}