using System;

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

        public bool HasBucket { get; private set; }

        private void Start()
        {
            HasBucket = true;
        }

        public override void Slap()
        {
            if (HasBucket == true)
            {
                BucketHitEvent?.Invoke(BucketHitResult.Stagger);
                return;
            }
            
            base.Slap();
        }

        public override void Snap()
        {
            if (HasBucket == true)
            {
                BucketHitEvent?.Invoke(BucketHitResult.Stun);
                return;
            }
            
            base.Snap();
        }

        public override void GigaSnap()
        {
            if (HasBucket == true)
            {
                BucketHitEvent?.Invoke(BucketHitResult.Breached);
                HasBucket = false;
                return;
            }
            
            base.GigaSnap();
        }
    }
}
