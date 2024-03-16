using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

[UsedImplicitly]
public class KeyboardInput : IInputSystem
{
    /// <summary>
    /// Contains keycodes for the InputActions
    /// </summary>
    private readonly Dictionary<InputAction, KeyCode> _keyCodes = new()
    {
        { InputAction.MoveUp, KeyCode.W },
        { InputAction.MoveDown, KeyCode.S },
        { InputAction.MoveLeft, KeyCode.A },
        { InputAction.MoveRight, KeyCode.D },
        { InputAction.Jump, KeyCode.Space }
    };
    
    public Vector2 GetMovementDirection()
    {
        return new Vector2(GetHorizontalAxisRaw(), GetVerticalAxisRaw());
    }

    public bool IsActionPressed(InputAction action)
    {
        if (_keyCodes.TryGetValue(action, out KeyCode keyCode) == true)
        {
            return Input.GetKey(keyCode);
        }

        Debug.LogError($"Can`t find KeyCode for {action} action.");
        return false;
    }

    private float GetHorizontalAxisRaw()
    {
        int left = IsActionPressed(InputAction.MoveLeft) ? -1 : 0;
        int right = IsActionPressed(InputAction.MoveRight) ? 1 : 0;

        return left + right;
    }

    private float GetVerticalAxisRaw()
    {
        int down = IsActionPressed(InputAction.MoveDown) ? -1 : 0;
        int up = IsActionPressed(InputAction.MoveUp) ? 1 : 0;

        return down + up;
    }
}