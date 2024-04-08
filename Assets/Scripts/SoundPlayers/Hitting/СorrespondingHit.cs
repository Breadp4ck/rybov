using System.Collections.Generic;
using Units.Hitting;
using UnityEngine;

namespace SoundPlayers.Hitting
{
    public class СorrespondingHit : SoundPlayer
    {
        [SerializeField] protected Hittable _hittable;
        
        [SerializeField] private List<AudioClip> _slapSounds;
        [SerializeField] private List<AudioClip> _snapSounds;
        [SerializeField] private List<AudioClip> _gigaSnapSounds;
        
        private void OnEnable()
        {
            _hittable.HitEvent += OnHit;
        }

        private void OnDisable()
        {
            _hittable.HitEvent -= OnHit;
        }
        
        protected virtual void OnHit(HitType hitType)
        {
            switch (hitType)
            {
                case HitType.Slap:
                    PlaySound(_slapSounds[Random.Range(0, _slapSounds.Count)]);
                    break;
                case HitType.Snap:
                    PlaySound(_snapSounds[Random.Range(0, _snapSounds.Count)]);
                    break;
                case HitType.GigaSnap:
                    PlaySound(_gigaSnapSounds[Random.Range(0, _gigaSnapSounds.Count)]);
                    break;
            }
        }
    }
}
