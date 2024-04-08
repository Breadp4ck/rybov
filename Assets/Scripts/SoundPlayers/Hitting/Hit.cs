using System.Collections.Generic;
using Units.Hitting;
using UnityEngine;
using Random = UnityEngine.Random;

namespace SoundPlayers.Hitting
{
    public class Hit : SoundPlayer
    {
        [SerializeField] private List<AudioClip> _hitSounds;
        
        [SerializeField] private Hittable _hittable;

        private void OnEnable()
        {
            _hittable.HitEvent += OnHit;
        }

        private void OnDisable()
        {
            _hittable.HitEvent -= OnHit;
        }

        protected void OnHit(HitType hitType)
        {
            PlaySound(_hitSounds[Random.Range(0, _hitSounds.Count)]);
        }
    }
}