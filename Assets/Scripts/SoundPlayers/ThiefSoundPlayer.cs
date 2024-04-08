using System;
using System.Collections.Generic;
using Units;
using UnityEngine;
using Random = UnityEngine.Random;

namespace SoundPlayers
{
    public class ThiefSoundPlayer : SoundPlayer
    {
        [SerializeField] private List<AudioClip> _thiefLaughSounds;

        // Should be IFishThief, but it's can't be SerializeField
        [SerializeField] private Cat _thief;
        
        private void OnEnable()
        {
            _thief.FishStoleEvent += OnFishStole;   
        }
        
        private void OnDisable()
        {
            _thief.FishStoleEvent -= OnFishStole;
        }

        private void OnFishStole()
        {
            PlaySound(_thiefLaughSounds[Random.Range(0, _thiefLaughSounds.Count)]);
        }
    }
}