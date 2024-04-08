using System;

namespace Units.Hitting
{
    public class Stone : Hittable
    {
        public override event Action<HitType> HitEvent;
        
        public override void OnHit(float power)
        {
            HitEvent?.Invoke(HitType.Slap);
        }
    }
}
