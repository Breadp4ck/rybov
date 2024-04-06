using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Fishing.Fish;
using Fishing.Handlers;
using Units.Spawning;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Fishing.Pool
{
    public class FishLake : MonoBehaviour, IFishLake
    {
        public event Action FishGeneratedEvent;
        public event Action StartCatchingEvent;
        public event Action<CatchHandler.CatchResult> EndCatchingEvent;

        /// <summary>
        /// If player trying to catch the fish.
        /// </summary>
        public bool IsCatching { get; private set; }

        public FishInfo AvailableFishInfo { get; private set; }
        
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

                float currentCatchExtent = AvailableFishInfo.InitialCatchExtent;
                while (IsCatching == false)
                {
                    currentCatchExtent -= AvailableFishInfo.MaxLoseSpeed / 10 * Time.deltaTime;
                    if (currentCatchExtent <= 0f)
                    {
                        StopCatching(CatchHandler.CatchResult.Fail);
                        break;
                    }
                
                    yield return null;
                }

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