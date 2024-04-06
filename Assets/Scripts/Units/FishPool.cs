using System;
using System.Collections.Generic;
using UnityEngine;

namespace Units.Spawning
{
    /// <summary>
    /// Handler of all StealableFish instances.
    /// </summary>
    public static class FishPool
    {
        public static event Action<StealableFish> FishCaughtEvent;
        public static event Action<StealableFish> FishStolenEvent;
        public static event Action<StealableFish> FishDroppedEvent;

        public static List<StealableFish> FreeFishes { get; } = new();
        public static Dictionary<StealableFish, IFishThief> StolenFishes { get; } = new();

        public static void CatchFish(StealableFish fish)
        {
            FreeFishes.Add(fish);
            
            FishCaughtEvent?.Invoke(fish);
        }
        
        public static bool TryStealFish(StealableFish fish, IFishThief thief)
        {
            if (StolenFishes.ContainsKey(fish) == true)
            {
                return false;
            }
            
            fish.OnSteal(thief);
            thief.OnFishSteal(fish);
            
            FreeFishes.Remove(fish);
            StolenFishes.Add(fish, thief);
            
            FishStolenEvent?.Invoke(fish);

            return true;
        }
        
        public static void DropFish(StealableFish fish, IFishThief thief)
        {
            fish.OnDrop();
            thief.OnFishDrop();

            StolenFishes.Remove(fish);
            FreeFishes.Add(fish);
            
            FishDroppedEvent?.Invoke(fish);
        }

        /// <summary>
        /// Carry off the fish from the GameFieldBorder.
        /// </summary>
        public static void CarryAwayFish(StealableFish fish)
        {
            StolenFishes.Remove(fish);
        }
        
        /// <summary>
        /// Invoked when the fish is out of the GameFieldBorder by itself. 
        /// </summary>
        public static void OutOfBorder(StealableFish fish)
        {
            FreeFishes.Remove(fish);
        }

        public static StealableFish GetClosestTo(Vector2 position)
        {
            StealableFish closestStealableFish = null;
            var closestDistance = float.MaxValue;

            foreach (StealableFish fish in FreeFishes)
            {
                float distance = Vector2.Distance(position, fish.transform.position);
                if (distance >= closestDistance)
                {
                    continue;
                }

                closestStealableFish = fish;
                closestDistance = distance;
            }

            return closestStealableFish;
        }
    }
}
