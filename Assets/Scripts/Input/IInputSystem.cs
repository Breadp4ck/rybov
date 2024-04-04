namespace Inputs
{
    public interface IInputSystem
    {
        /// <summary>
        /// Check if the button`s associated action is pressed
        /// </summary>
        /// <param name="action">Action to check</param>
        /// <returns>True if the action is pressed, false otherwise</returns>
        bool IsActionDown(InputAction action);

        /// <summary>
        /// Check if the button`s associated action is pressed at this frame
        /// </summary>
        /// <param name="action">Action to check</param>
        /// <returns>True if the action is pressed, false otherwise</returns>
        bool IsActionPressed(InputAction action);

        /// <summary>
        /// Check if the button`s associated action is released at this frame
        /// </summary>
        /// <param name="action">Action to check</param>
        /// <returns>True if the action is released this frame, false otherwise</returns>
        bool IsActionUp(InputAction action);
    }
}