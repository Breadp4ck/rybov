using System;
using System.Collections.Generic;
using Units.Hitting;
using UnityEngine;

namespace GlobalState.Scores
{
    [CreateAssetMenu(fileName = "HitScoresDistribution", menuName = "ScoresDistribution/Hit")]
    public class HitScoresDistribution : ScriptableObject
    {
        [System.Serializable]
        private class Info
        { 
            public HitType Type => _type;
            [SerializeField] private HitType _type;
            
            public uint Score => _score;
            [SerializeField] private uint _score;

            public Info(HitType type, uint score)
            {
                _type = type;
                _score = score;
            }
        }
        
        [SerializeField] private List<Info> _scores;

        private void OnValidate()
        {
            Array hitTypes = Enum.GetValues(typeof(HitType));
            if (_scores.Count == hitTypes.Length)
            {
                return;
            }

            _scores.Clear();
            for (var i = 0; i < hitTypes.Length; i++)
            {
                _scores.Add(new Info((HitType) hitTypes.GetValue(i), 0));
            }
        }

        public uint GetScore(HitType type)
        {
            return _scores.Find(info => info.Type == type).Score;
        }
    }
}