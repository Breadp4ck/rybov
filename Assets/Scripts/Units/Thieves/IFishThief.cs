using UnityEngine;

namespace Units
{
    public interface IFishThief
    {
        GameObject GameObject { get; }
        
        StealableFish CarriedStealableFish { get; }
        
        void Steal(StealableFish fish);
        void Drop(StealableFish fish);
    }
}