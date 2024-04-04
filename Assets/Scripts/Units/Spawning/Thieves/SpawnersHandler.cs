﻿using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Units.Spawning
{
    public class SpawnersHandler : MonoBehaviour
    {
        [SerializeField] private List<SpawnInfo> _spawnInfo;

        [SerializeField] private List<FishThiefSpawner> _spawners;

        [SerializeField] private uint _wavesCount;

        [SerializeField] private float _wavesIntervalSeconds;
        [SerializeField] private float _spawnsIntervalSeconds;

        private IEnumerator _spawnThievesWavesRoutine;
        
        private void OnValidate()
        {
            _spawners = FindObjectsOfType<FishThiefSpawner>().ToList();
        }

        private void OnEnable()
        {
            // TODO: Subscribe to #13.
            // += OnFishCaught();
        }

        private void OnDisable()
        {
            // TODO: UnSubscribe to #13.
            // -= OnFishCaught();
            
            if (_spawnThievesWavesRoutine != null)
            {
                StopCoroutine(_spawnThievesWavesRoutine);
            }
        }

        private void OnFishCaught()
        {
            if (_spawnThievesWavesRoutine != null)
            {
                StopCoroutine(_spawnThievesWavesRoutine);
            }

            _spawnThievesWavesRoutine = SpawnThievesWaves();
            StartCoroutine(_spawnThievesWavesRoutine);
        }

        private IEnumerator SpawnThievesWaves()
        {
            for (var i = 0; i < _wavesCount; i++)
            {
                uint balance = GetBalance();
                while (balance > _spawnInfo.Select(x => x.SpawnCost).Min())
                {
                    SpawnInfo spawnInfo = GetRelativelyRandomSpawnInfoByBalance(balance);
                    GetRandomSpawner().Spawn(spawnInfo.FishThiefPrefab);

                    balance -= spawnInfo.SpawnCost;
                    yield return new WaitForSeconds(_spawnsIntervalSeconds);
                }

                yield return new WaitForSeconds(_wavesIntervalSeconds);
            }
        }

        /// <summary>
        /// Get balance for the current wave
        /// Balance can be spent on spawning thieves
        /// </summary>
        /// <returns>Balance value</returns>
        private uint GetBalance()
        {
            // TODO: Implement balance calculation via some math function
            return 0;
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
        
        private FishThiefSpawner GetRandomSpawner()
        {
            return _spawners[Random.Range(0, _spawners.Count)];
        }
    }
}