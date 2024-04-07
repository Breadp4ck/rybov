using System;
using Units;
using UnityEngine;

namespace Units
{
    public class BucketCat : Cat
    {
        public enum BucketHitResult : byte
        {
            NotBreached,
            Stagger,
            Stun,
            Breached
        }

        public event Action<BucketHitResult> BucketHitEvent;

        private bool _hasBucket;

        private void Start()
        {
            _hasBucket = true;
        }

        public override void Slap()
        {
            if (_hasBucket == true)
            {
                BucketHitEvent?.Invoke(BucketHitResult.Stagger);
                return;
            }
            
            base.Slap();
        }

        public override void Snap()
        {
            if (_hasBucket == true)
            {
                BucketHitEvent?.Invoke(BucketHitResult.Stun);
                return;
            }
            
            base.Snap();
        }

        public override void GigaSnap()
        {
            if (_hasBucket == true)
            {
                BucketHitEvent?.Invoke(BucketHitResult.Breached);
                _hasBucket = false;
                return;
            }
            
            base.GigaSnap();
        }
    }
}
