using System;
using UnityEngine;

namespace Units.Hitting
{
    public abstract class Hittable : MonoBehaviour
    {
        public static Action<HitType> StaticHitEvent;
        
        public abstract event Action<HitType> HitEvent;
        
        public abstract void OnHit(float power);

        public virtual void GigaSnap() { }

        public virtual void Snap() { }

        public virtual void Slap() { }
    }
}
