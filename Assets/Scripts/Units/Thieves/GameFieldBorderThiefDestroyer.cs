using System;
using Units;
using Units.Spawning;
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
        
        FishPool.CarryAwayFish(thief.CarriedStealableFish);
        
        Destroy(thief.GameObject);
    }
}
