using System.Collections.Generic;
using Units;
using UnityEngine;

namespace SoundPlayers.Hitting
{
    public class BucketCatHit : СorrespondingHit
    {
        [SerializeField] private BucketCat _bucketCat;
        
        [SerializeField] private List<AudioClip> _bucketMetalSounds;

        private void OnEnable()
        {
            _hittable.HitEvent += OnHit;
            _bucketCat.BucketHitEvent += OnBucketHit;
        }

        private void OnDisable()
        {
            _hittable.HitEvent -= OnHit;
            _bucketCat.BucketHitEvent -= OnBucketHit;
        }

        private void OnBucketHit(BucketCat.BucketHitResult bucketHitResult)
        {
            PlaySound(_bucketMetalSounds[Random.Range(0, _bucketMetalSounds.Count)]);
        }
    }
}