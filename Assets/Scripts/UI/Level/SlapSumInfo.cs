using System.Collections.Generic;
using System.Linq;
using GlobalState.Scores;
using GlobalStates.Game;
using TMPro;
using Units;
using UnityEngine;

namespace UI.Level
{
    public class SlapSumInfo : MonoBehaviour
    {
        private Scores Scores => Scores.Instance;

        [SerializeField] private TextMeshProUGUI _text;

        private void Update()
        {
            _text.text = $"{Scores.HitsScore.Sum(x => x.Value)}";
        }   
    }
}