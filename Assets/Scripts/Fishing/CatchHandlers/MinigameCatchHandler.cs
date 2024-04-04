using System;
using System.Collections;
using Fishing.Fish;
using UI.Fishing.Handlers;
using UnityEngine;

namespace Fishing.Handlers
{
    public class MinigameCatchHandler : CatchHandler
    {
        public override event Action<CatchResult> CatchFinishedEvent;
        
        [SerializeField] private MinigameCatchHandlerUI _catchHandlerUI;

        public override float CurrentCatchExtent { get; protected set; }
        public override float MaxCatchExtent { get; protected set; }

        private IEnumerator _handleCatchRoutine;
        
        public override void StartCatching(FishInfo fishInfo)
        {
            if (_handleCatchRoutine != null)
            {
                StopCoroutine(_handleCatchRoutine);
            }

            _handleCatchRoutine = HandleCatch(fishInfo); 
            StartCoroutine(_handleCatchRoutine);
        }

        public override void StopCatching()
        {
            if (_handleCatchRoutine != null)
            {
                StopCoroutine(_handleCatchRoutine);
            }
        }

        private IEnumerator HandleCatch(FishInfo fishInfo)
        {
            CurrentCatchExtent = fishInfo.InitialCatchExtent;
            while (CurrentCatchExtent > 0f)
            {
                // TODO: TESTING
                
                // From -1 to 1
                // Определить по направлению тяги удочки и направлению движения рыбы
                var coef = 1f;
                
                // MaxCatchSpeed or MaxLoseSpeed
                CurrentCatchExtent += coef * fishInfo.MaxCatchSpeed * Time.fixedDeltaTime;
                
                if (CurrentCatchExtent >= 1f)
                {
                    CatchFinishedEvent?.Invoke(CatchResult.Success);
                    yield break;
                }
                
                yield return new WaitForFixedUpdate();
            }
            
            CatchFinishedEvent?.Invoke(CatchResult.Fail);
        }
    }
}