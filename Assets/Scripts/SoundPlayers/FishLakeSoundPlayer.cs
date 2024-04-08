using System.Collections.Generic;
using Fishing;
using Fishing.Handlers;
using Fishing.Pool;
using UnityEngine;

namespace SoundPlayers
{
    public class FishLakeSoundPlayer : SoundPlayer
    {
        [SerializeField] private FishLake _fishLake;

        [SerializeField] private List<AudioClip> _reelSounds;
        [SerializeField] private List<AudioClip> _successSounds;
        
        private void OnEnable()
        {
            _fishLake.StartCatchingEvent += OnStartCatching;
            _fishLake.EndCatchingEvent += OnEndCatching;
        }

        private void OnDisable()
        {
            _fishLake.StartCatchingEvent -= OnStartCatching;
            _fishLake.EndCatchingEvent -= OnEndCatching;
        }
        
        private void OnStartCatching()
        {
            PlaySound(_reelSounds[Random.Range(0, _reelSounds.Count)], true);
        }
        
        private void OnEndCatching(CatchHandler.CatchResult catchResult)
        {
            if (catchResult != CatchHandler.CatchResult.Success)
            {
                return;
            }
            
            PlaySound(_successSounds[Random.Range(0, _successSounds.Count)]);
        }
    }
}