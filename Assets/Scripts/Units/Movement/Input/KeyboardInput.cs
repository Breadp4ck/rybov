using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

[UsedImplicitly]
public class KeyboardInput : IInputSystem
{
    public Vector2 LookDirection { get; private set; } = new(1, 0);
    
    /// <summary>
    /// Contains keycodes for the InputActions
    /// </summary>
    private readonly Dictionary<InputAction, KeyCode> _keyCodes = new()
    {
        { InputAction.MoveUp, KeyCode.W },
        { InputAction.MoveDown, KeyCode.S },
        { InputAction.MoveLeft, KeyCode.A },
        { InputAction.MoveRight, KeyCode.D },
        
        { InputAction.Dash, KeyCode.LeftControl },
        { InputAction.Run, KeyCode.LeftShift}
    };
    
    public Vector2 GetMovementDirection()
    {
        Vector2 direction = new(GetHorizontalAxisRaw(), GetVerticalAxisRaw());

        if (direction.SqrMagnitude() > 0)
        {
            LookDirection = direction.normalized;
        }

        return direction.normalized;
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