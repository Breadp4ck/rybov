using System.Linq;
using GlobalState.Scores;
using TMPro;
using UnityEngine;

namespace UI.Level
{
    public class ScoreSummer : MonoBehaviour
    {
        private Scores Scores => Scores.Instance;

        [SerializeField] private TextMeshProUGUI _text;

        private void Update()
        {
            long totalScore = 0;
            
            long catchedFishScore = Scores.CatchedFishScore.Sum(x => x.Value);
            long savedFishScore = Scores.SavedFishScore.Sum(x => x.Value);
            long hitsScore = Scores.HitsScore.Sum(x => x.Value);
            
            totalScore += catchedFishScore;
            totalScore += savedFishScore;
            totalScore += hitsScore;
            _text.text = $"{totalScore}";
        }
    }
}