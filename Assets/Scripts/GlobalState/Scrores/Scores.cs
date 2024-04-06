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

        public List<FishScoreInfo> QuickNibbleScores { get; private set; } = new();
        public List<FishScoreInfo> CatchedFishScore { get; private set; } = new();
        public List<FishScoreInfo> SavedFishScore => GetSavedFishScoreInfo();

        public List<HitScoresInfo> HitsScore { get; private set; } = new();

        [Header("Fish Scores Distribution")]
        [SerializeField] private FishScoresDistribution _quickNibbleScoresDistribution;
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
            DontDestroyOnLoad(gameObject);
        }

        private void OnDestroy()
        {
            Instance = null;
        }

        private void OnEnable()
        {
            // TODO: Subscribe to #13
            // Быстрое реагирование на клёв рыбы
            
            // Вылов рыбы
            FishPool.FishCaughtEvent += OnFishCaught;
            
            // Шлепки/щелбаны/убер-щелбаны по котам
            IHittable.HitEvent += OnHit;
        }

        private void OnDisable()
        {
            // TODO: Unsubscribe from #13
            // Быстрое реагирование на клёв рыбы
            
            // Вылов рыбы
            FishPool.FishCaughtEvent -= OnFishCaught;
            
            // Шлепки/щелбаны/убер-щелбаны по котам
            IHittable.HitEvent -= OnHit;
            
        }

        // TODO: DEBUG
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.S))
            {
                long totalScore = 0;
                
                long quickNibbleScore = QuickNibbleScores.Sum(x => x.Value);
                long catchedFishScore = CatchedFishScore.Sum(x => x.Value);
                long savedFishScore = SavedFishScore.Sum(x => x.Value);
                long hitsScore = HitsScore.Sum(x => x.Value);
                
                
                totalScore += quickNibbleScore;
                totalScore += catchedFishScore;
                totalScore += savedFishScore;
                totalScore += hitsScore;
                
                Debug.Log(
                    $"QuickNibbleScore: {quickNibbleScore} " +
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