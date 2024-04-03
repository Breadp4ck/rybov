using TMPro;
using Units.Spawning;
using UnityEngine;

/// <summary>
/// Debug ONLY. 
/// </summary>
public class FishPoolCountUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _text;

    private void Update()
    {
        _text.text = $"{FishPool.FreeFishes.Count} | {FishPool.StolenFishes.Count}";
    }
}
