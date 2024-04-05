using Units;
using Units.Hitting;

namespace GlobalState.Scores
{
    public struct FishScoreInfo
    {
        public StealableFish.Type FishType { get; private set; }
        
        public uint Value { get; private set; }

        public FishScoreInfo(StealableFish.Type fishType, uint value)
        {
            FishType = fishType;
            Value = value;
        }
    }
    
    public struct HitScoresInfo
    {
        public HitType HitType { get; private set; }

        public uint Value { get; private set; }

        public HitScoresInfo(HitType hitType, uint value)
        {
            HitType = hitType;
            Value = value;
        }

    }
}