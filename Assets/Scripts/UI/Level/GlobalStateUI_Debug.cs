using GlobalStates.Game;
using TMPro;
using UnityEngine;

public class GlobalStateUI_Debug : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _text;

    private void Update()
    {
        _text.text = $"{Game.Instance.CurrentState.Type}";
    }
}
