using System;
using System.Collections.Generic;
using Fishing;
using Fishing.Handlers;
using Fishing.Pool;
using UnityEngine;
using Random = UnityEngine.Random;

namespace SoundPlayers
{
    public class FishLakeSoundPlayer : SoundPlayer
    {
        [SerializeField] private FishLake _fishLake;

        [SerializeField] private List<AudioClip> _reelSounds;
        [SerializeField] private List<AudioClip> _successSounds;
        [SerializeField] private List<AudioClip> _failSounds;
        
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
            switch (catchResult)
            {
                case CatchHandler.CatchResult.Fail:
                    PlaySound(_failSounds[Random.Range(0, _failSounds.Count)]);
                    break;
                case CatchHandler.CatchResult.Success:
                    PlaySound(_successSounds[Random.Range(0, _successSounds.Count)]);
                    break;
            }
        }
    }
}