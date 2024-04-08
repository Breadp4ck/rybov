using System;
using Units.Hitting;

public class Grass : Hittable
{
    public override event Action<HitType> HitEvent;
    
    public override void OnHit(float power)
    {
        HitEvent?.Invoke(HitType.Slap);
    }
}
