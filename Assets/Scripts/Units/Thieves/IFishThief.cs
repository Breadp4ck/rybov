using System;
using Units.Destroying;

namespace Units
{
    public interface IFishThief : IOutOfBorderInteractable
    {
        event Action FishStoleEvent;
        event Action FishDroppedEvent;
        
        public StealableFish CarriedFish { get; }
        
        void OnFishSteal(StealableFish fish);
        void OnFishDrop();
    }
}