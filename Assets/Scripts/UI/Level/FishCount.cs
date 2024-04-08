using TMPro;
using Units.Spawning;
using UnityEngine;

namespace UI.Level
{
    public class FishCount : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _text;

        private void Update()
        {
            _text.text = $"{FishPool.FreeFishes.Count}";
        }
    }
}