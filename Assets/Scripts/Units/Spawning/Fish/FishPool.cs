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
        public static event Action FishCaughtEvent;
        public static event Action FishStolenEvent;
        public static event Action FishDroppedEvent;

        public static List<StealableFish> FreeFishes { get; } = new();
        public static List<StealableFish> StolenFishes { get; } = new();

        public static void CatchFish(StealableFish fish)
        {
            FreeFishes.Add(fish);
            
            FishCaughtEvent?.Invoke();
        }
        
        public static void StealFish(StealableFish fish, IFishThief thief)
        {
            fish.Thief = thief;
            thief.Steal(fish);
            
            FreeFishes.Remove(fish);
            StolenFishes.Add(fish);
            
            FishStolenEvent?.Invoke();
        }
        
        public static void DropFish(StealableFish fish)
        {
            fish.Thief.Drop(fish);
            fish.Thief = null;

            StolenFishes.Remove(fish);
            FreeFishes.Add(fish);
            
            FishDroppedEvent?.Invoke();
        }

        /// <summary>
        /// Carry off the fish from the GameFieldBorder.
        /// </summary>
        public static void CarryAwayFish(StealableFish fish)
        {
            StolenFishes.Remove(fish);
            
            UnityEngine.Object.Destroy(fish.gameObject);
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
