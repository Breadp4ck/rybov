using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

namespace Inputs
{
    [UsedImplicitly]
    public class KeyboardInput : IInputSystem
    {
        /// <summary>
        /// Contains keycodes for the InputActions
        /// </summary>
        private readonly Dictionary<InputAction, KeyCode> _keyCodes = new()
        {
            { InputAction.LeftClick, KeyCode.Mouse0 },
            { InputAction.RightClick, KeyCode.Mouse1 },
            { InputAction.PauseToggle, KeyCode.Escape }
        };

        public bool IsActionDown(InputAction action)
        {
            if (_keyCodes.TryGetValue(action, out KeyCode keyCode) == true)
            {
                return Input.GetKeyDown(keyCode);
            }

            Debug.LogError($"Can`t find KeyCode for {action} action.");
            return false;
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

        public bool IsActionUp(InputAction action)
        {
            if (_keyCodes.TryGetValue(action, out KeyCode keyCode) == true)
            {
                return Input.GetKeyUp(keyCode);
            }

            Debug.LogError($"Can`t find KeyCode for {action} action.");
            return false;
        }
    }
}