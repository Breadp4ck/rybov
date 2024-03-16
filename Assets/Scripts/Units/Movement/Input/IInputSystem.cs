using UnityEngine;

public interface IInputSystem
{
    /// <summary>
    /// Get normalized direction of the movement
    /// </summary>
    /// <returns>Normalized Vector3 of the movement</returns>
    Vector2 GetMovementDirection();

    /// <summary>
    /// Check if the button associated action is pressed
    /// </summary>
    /// <param name="action">Action to check</param>
    /// <returns>True if the action is pressed, false otherwise</returns>
    bool IsActionPressed(InputAction action);
}