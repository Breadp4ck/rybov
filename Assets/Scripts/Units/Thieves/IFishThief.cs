using Units.Destroying;

namespace Units
{
    public interface IFishThief : IOutOfBorderInteractable
    {
        public StealableFish CarriedFish { get; }
        
        void OnFishSteal(StealableFish fish);
        void OnFishDrop();
    }
}