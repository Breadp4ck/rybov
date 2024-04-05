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
        
        [SerializeField] private MinigameCatchHandlerUI _catchHandlerUI;

        [SerializeField] private FishingRod _fishingRod;

        [SerializeField] private Transform _fishTransform;

        [SerializeField] private float _radius;

        public override float CurrentCatchExtent { get; protected set; }
        public override float MaxCatchExtent { get; protected set; }

        private IEnumerator _handleCatchRoutine;

        private Vector3 _fishDirectionPull;
        private Vector3 _fishInitialPosition;

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(_fishTransform.position, _radius);
        }

        public void Awake()
        {
            _fishInitialPosition = _fishTransform.transform.position;
        }

        public override void StartCatching(FishInfo fishInfo)
        {
            _fishTransform.transform.position = _fishInitialPosition;
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
            _fishTransform.transform.position = new Vector3(-5f, 2.25f, 0f);

            CurrentCatchExtent = fishInfo.InitialCatchExtent;
            while (CurrentCatchExtent > 0f)
            {
                var rodNormalized = (_fishingRod.transform.position - _fishingRod.InitialPosition) / _fishingRod.Radius;
                rodNormalized.z = 0f;
                var fishNormalized = (_fishTransform.transform.position - _fishInitialPosition) / _radius;
                fishNormalized.z = 0f;

                // TODO: TESTING

                // From -1 to 1
                // Определить по направлению тяги удочки и направлению движения рыбы
                //var coef = Vector2.Dot(rodNormalized, fishNormalized);
                var angle = Vector2.Angle(rodNormalized, fishNormalized);
                if (angle >= 150)
                {
                    CurrentCatchExtent += (180 - angle) / 30 * fishInfo.MaxCatchSpeed * Time.fixedDeltaTime;
                }

                else
                {
                    CurrentCatchExtent -= (150 - angle) / 150f * fishInfo.MaxLoseSpeed * Time.fixedDeltaTime;
                }

                // MaxCatchSpeed or MaxLoseSpeed
                //CurrentCatchExtent += coef * fishInfo.MaxCatchSpeed * Time.fixedDeltaTime;
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