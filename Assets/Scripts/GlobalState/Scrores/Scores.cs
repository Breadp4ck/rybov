using System.Collections.Generic;
using System.Linq;
using Units;
using Units.Hitting;
using Units.Spawning;
using UnityEngine;

namespace GlobalState.Scores
{
    public class Scores : MonoBehaviour
    {
        public Scores Instance { get; private set; }

        public List<FishScoreInfo> CatchedFishScore { get; private set; } = new();
        public List<FishScoreInfo> SavedFishScore => GetSavedFishScoreInfo();

        public List<HitScoresInfo> HitsScore { get; private set; } = new();

        [Header("Fish Scores Distribution")]
        [SerializeField] private FishScoresDistribution _catchedFishScoresDistribution;
        [SerializeField] private FishScoresDistribution _savedFishScoresDistribution;
        
        [Header("Hit Scores Distribution")]
        [SerializeField] private HitScoresDistribution _hitScoresDistribution;

        private void Awake()
        {
            if (Instance != null)
            {
                Destroy(gameObject);
                return;
            }
            
            Instance = this;
        }

        private void OnEnable()
        {
            // Вылов рыбы
            FishPool.FishCaughtEvent += OnFishCaught;
            
            // Шлепки/щелбаны/убер-щелбаны по котам
            IHittable.StaticHitEvent += OnHit;
        }

        private void OnDisable()
        {
            // Вылов рыбы
            FishPool.FishCaughtEvent -= OnFishCaught;
            
            // Шлепки/щелбаны/убер-щелбаны по котам
            IHittable.StaticHitEvent -= OnHit;
            
        }

        // TODO: DEBUG
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.S))
            {
                long totalScore = 0;
                
                long catchedFishScore = CatchedFishScore.Sum(x => x.Value);
                long savedFishScore = SavedFishScore.Sum(x => x.Value);
                long hitsScore = HitsScore.Sum(x => x.Value);
                
                
                totalScore += catchedFishScore;
                totalScore += savedFishScore;
                totalScore += hitsScore;
                
                Debug.Log(
                    $"CatchedFishScore: {catchedFishScore} " +
                    $"SavedFishScore: {savedFishScore} " +
                    $"HitsScore: {hitsScore} " +
                    $"TOTAL: {totalScore}");
            }
        }

        private void OnFishCaught(StealableFish fish)
        {
            CatchedFishScore.Add(new FishScoreInfo(fish.FishType, _catchedFishScoresDistribution.GetScore(fish.FishType)));
        }
        
        private List<FishScoreInfo> GetSavedFishScoreInfo()
        {
            List<FishScoreInfo> savedFishScore = new();
            foreach (StealableFish freeFish in FishPool.FreeFishes)
            {
                FishScoreInfo fishScoreInfo = new(freeFish.FishType, _savedFishScoresDistribution.GetScore(freeFish.FishType));
                savedFishScore.Add(fishScoreInfo);
            }

            return savedFishScore;
        }
        
        private void OnHit(HitType hitType)
        {
            HitsScore.Add(new HitScoresInfo(hitType, _hitScoresDistribution.GetScore(hitType)));
        }
    }
}