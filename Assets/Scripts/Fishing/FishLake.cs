using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Fishing.Fish;
using Fishing.Handlers;
using Units.Hitting;
using Units.Spawning;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Fishing.Pool
{
    public class FishLake : Hittable
    {
        public override event Action<HitType> HitEvent;
        
        public event Action FishGeneratedEvent;
        public event Action StartCatchingEvent;
        public event Action<CatchHandler.CatchResult> EndCatchingEvent;

        /// <summary>
        /// If player trying to catch the fish.
        /// </summary>
        public bool IsCatching { get; private set; }

        public FishInfo AvailableFishInfo { get; private set; }

        public CatchHandler CatchHandler => _catchHandler;
        [SerializeField] private CatchHandler _catchHandler;
        
        [SerializeField] private List<FishInfo> _fishInfo;

        private ISpawner _fishSpawner;
        
        /// <summary>
        /// How long player will wait to catch the fish.
        /// </summary>
        [Header("Fishing time")] 
        [SerializeField] private float _minFishGenerateIntervalSeconds;
        [SerializeField] private float _maxFishGenerateIntervalSeconds;

        private void Awake()
        {
            _fishSpawner = GetComponentInChildren<ISpawner>();
        }

        private void Start()
        {
            StartCoroutine(GenerateFish());
        }
        
        private void OnValidate()
        {
            if (Math.Abs(_fishInfo.Select(x => x .SpawnChance).Sum() - 1) > 0.01f)
            {
                Debug.LogWarning($"Spawn chances in {gameObject} sum are not equal to 1");
            }
        }

        public void StartCatching()
        {
            if (IsCatching == true)
            {
                Debug.LogError("Already catching.");
                return;
            }
            
            if (AvailableFishInfo == null)
            {
                Debug.LogError("No fish to catch");
                return;
            }
            
            // Catch
            _catchHandler.CatchFinishedEvent += OnCatchFinished;
            _catchHandler.StartCatching(AvailableFishInfo);
            
            StartCatchingEvent?.Invoke();
            
            IsCatching = true;
            
            Debug.Log("Start catching.");
        }

        public void StopCatching(CatchHandler.CatchResult result)
        {
            EndCatchingEvent?.Invoke(result);
            
            _catchHandler.CatchFinishedEvent -= OnCatchFinished;
            _catchHandler.StopCatching();
            
            AvailableFishInfo = null;
            IsCatching = false;
            
            Debug.Log("Stop catching.");
        }
        
        public override void OnHit(float power)
        {
            HitEvent?.Invoke(HitType.Slap);
        }
        
        private void OnCatchFinished(CatchHandler.CatchResult result)
        {
            if (result == CatchHandler.CatchResult.Success)
            {
                _fishSpawner.Spawn(AvailableFishInfo.StealableFish.gameObject);
            }
            
            StopCatching(result);
        }
        
        private IEnumerator GenerateFish()
        {
            while (true)
            {
                TimeSpan generateTime = TimeSpan.FromSeconds(Random.Range(_minFishGenerateIntervalSeconds, _maxFishGenerateIntervalSeconds));
                yield return new WaitForSeconds((float)generateTime.TotalSeconds);
            
                Debug.Log("Fish generated.");
                
                AvailableFishInfo = GetRelativelyRandomFishInfo();
                FishGeneratedEvent?.Invoke();

                yield return new WaitWhile(() => AvailableFishInfo != null);
            }
        }
        
        /// <summary>
        /// Get random fish info from the list of spawn info by SpawnInfo.SpawnChance
        /// </summary>
        /// <returns>Random SpawnInfo relatively to it`s spawn chance</returns>
        private FishInfo GetRelativelyRandomFishInfo()
        {
            float totalChance = _fishInfo.Sum(info => info.SpawnChance);
            float randomChance = Random.Range(0, totalChance);
            foreach (FishInfo info in _fishInfo)
            {
                randomChance -= info.SpawnChance;
                if (randomChance <= 0)
                {
                    return info;
                }
            }
            
            return _fishInfo[0];
        }
    }
}