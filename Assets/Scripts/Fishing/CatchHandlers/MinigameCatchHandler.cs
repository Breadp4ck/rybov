using System;
using System.Collections;
using Fishing.Fish;
using Fishing.Pool;
using MouseControls;
using UI.Fishing.Handlers;
using Units.Spawning;
using UnityEngine;
using UnityEngine.Serialization;

namespace Fishing.Handlers
{
    public class MinigameCatchHandler : CatchHandler
    {
        public override event Action<CatchResult> CatchFinishedEvent;
        
        [SerializeField] private FishingRod _fishingRod;

        [SerializeField] private FishMinigame _fishMinigame;

        [SerializeField] private float _radius;

        public override float CurrentCatchExtent { get; protected set; }
        public override float MaxCatchExtent { get; protected set; }

        private IEnumerator _handleCatchRoutine;


        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(_fishMinigame.transform.position, _radius);
        }

        public override void StartCatching(FishInfo fishInfo)
        {
            _fishMinigame.transform.position = _fishMinigame.FishTransform.position;
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
                _fishMinigame.FishDirectionPull = Vector3.zero;
                _fishMinigame.transform.position = _fishMinigame.FishTransform.position;
                StopCoroutine(_handleCatchRoutine);
            }
        }

        private IEnumerator HandleCatch(FishInfo fishInfo)
        {
            _fishMinigame.ChangeFishDirectionPull();
            var fishTime = 0f;

            CurrentCatchExtent = fishInfo.InitialCatchExtent;
            while (CurrentCatchExtent > 0f)
            {
                _fishMinigame.Update();
                _fishMinigame.transform.position = (Vector2)_fishMinigame.FishTransform.position +
                                                   Vector2.ClampMagnitude(
                                                       _fishMinigame.transform.position -
                                                       _fishMinigame.FishTransform.position, _radius);
                
                var rodNormalized = (_fishingRod.transform.position - _fishingRod.InitialPosition) / _fishingRod.Radius;
                rodNormalized.z = 0f;
                var fishNormalized =
                    (_fishMinigame.transform.position - _fishMinigame.FishTransform.position) / _radius +
                    (_fishMinigame.FishTransform.position - _fishingRod.transform.position);
                fishNormalized.z = 0f;

                // TODO: TESTING

                // From -1 to 1
                // Определить по направлению тяги удочки и направлению движения рыбы
                var angle = Vector2.Angle(rodNormalized, fishNormalized);
                if (angle >= 150f)
                {
                    CurrentCatchExtent += (180f - angle) / 30f * fishInfo.MaxCatchSpeed * Time.fixedDeltaTime;
                }

                else
                {
                    CurrentCatchExtent -= (150f - angle) / 150f * fishInfo.MaxLoseSpeed * Time.fixedDeltaTime;
                }

                fishTime += Time.fixedDeltaTime;
                if (fishTime > fishInfo.FishAggressivityInSeconds)
                {
                    fishTime = 0f;
                    _fishMinigame.ChangeFishDirectionPull();
                }

                print(CurrentCatchExtent);
                
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