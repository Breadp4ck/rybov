using System.Collections.Generic;
using GlobalState.Scores;
using GlobalStates.Game;
using TMPro;
using Units;
using UnityEngine;

namespace UI.Level
{
    public class SumOfCaughtFishInfoUI : MonoBehaviour
    {
        private Scores Scores => Scores.Instance;

        [SerializeField] private TextMeshProUGUI _text;
        [SerializeField] private StealableFish.Type _fishType;

        private (int, long) _info;

        private void Update()
        {
            _info = Scores.GetSavedFishTypeScoreInfo(_fishType); 
            _text.text = $"{_info.Item2}";
        }   
    }
}