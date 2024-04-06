using TMPro;
using Units.Spawning;
using UnityEngine;

public class FishStolenCountUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _text;

    private void Update()
    {
        _text.text = $"{FishPool.StolenFishes.Count}";
    }
}
