using System;
using Units;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class GameFieldBorderThiefDestroyer : MonoBehaviour
{
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.TryGetComponent(out IFishThief thief) == false)
        {
            return;
        }
        
        Destroy(thief.CarriedStealableFish.gameObject);
        Destroy(thief.GameObject);
    }
}
