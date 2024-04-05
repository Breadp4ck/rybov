using Units.Destroying;
using Units.Movement;
using UnityEngine;

namespace Units
{
    public interface IFishThief : IOutOfBorderInteractable
    {
        public StealableFish CarriedFish { get; }
        
        void OnFishSteal(StealableFish fish);
        void OnFishDrop();
    }
}