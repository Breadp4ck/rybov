using Fishing.Handlers;

namespace Fishing
{
    public interface IFishLake
    {
        bool IsCatching { get; }
        
        /// <summary>
        /// Is Fish ready to catch.
        /// </summary>
        // FishInfo AvailableFishInfo { get; }
        
        void StartCatching();
        void StopCatching(CatchHandler.CatchResult result);
    }
}