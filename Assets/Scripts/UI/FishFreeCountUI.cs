using TMPro;
using Units.Spawning;
using UnityEngine;

public class FishFreeCountUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _text;

    private void Update()
    {
        _text.text = $"{FishPool.FreeFishes.Count}";
    }
}
