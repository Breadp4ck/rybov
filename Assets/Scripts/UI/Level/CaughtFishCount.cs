using TMPro;
using Units.Spawning;
using UnityEngine;

namespace UI.Level
{
    public class CaughtFishCount : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _text;

        private void Update()
        {
            _text.text = $"{FishPool.StolenFishes.Count}";
        }
    }
}