using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(TextMeshProUGUI))]
public class PlayerMovementStateUI : MonoBehaviour
{
    [SerializeField] public TextMeshProUGUI _text;

    [SerializeField] private PlayerMovementStateMachine _player;
    
    private void Update()
    {
        _text.text = _player.CurrentState.ToString();
    }
}
