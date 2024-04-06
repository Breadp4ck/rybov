using System;

namespace Units.Hitting
{
    public interface IHittable
    {
        static Action<HitType> HitEvent;
        
        void OnHit(float power);
        
        void GigaSnap();
        void Snap();
        void Slap();
    }
}
