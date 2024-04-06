using System;
using System.Collections.Generic;
using Units;
using UnityEngine;

namespace GlobalState.Scores
{
    [CreateAssetMenu(fileName = "FishScoresDistribution", menuName = "ScoresDistribution/Fish")]
    public class FishScoresDistribution : ScriptableObject
    {
        [Serializable]
        private class Info
        { 
            public StealableFish.Type Type => _type;
            [SerializeField] private StealableFish.Type _type;
            
            public uint Score => _score;
            [SerializeField] private uint _score;

            public Info(StealableFish.Type type, uint score)
            {
                _type = type;
                _score = score;
            }
        }
        
        [SerializeField] private List<Info> _scores;
        
        private void OnValidate()
        {
            Array fishTypes = Enum.GetValues(typeof(StealableFish.Type));
            if (_scores.Count == fishTypes.Length)
            {
                return;
            }

            _scores.Clear();
            for (var i = 0; i < fishTypes.Length; i++)
            {
                _scores.Add(new Info((StealableFish.Type) fishTypes.GetValue(i), 0));
            }
        }
        
        public uint GetScore(StealableFish.Type type)
        {
            return _scores.Find(info => info.Type == type).Score;
        }
    }
}