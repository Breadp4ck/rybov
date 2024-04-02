using UnityEngine;

namespace Units.Destroying
{
    [RequireComponent(typeof(Collider2D))]

    public class GameFieldBorderThiefDestroyer : MonoBehaviour
    {
        private void OnTriggerExit2D(Collider2D other)
        {
            if (other.TryGetComponent(out IOutOfBorderInteractable interactable) == false)
            {
                return;
            }

            interactable.OnOutOfBorder();
        }
    }

}