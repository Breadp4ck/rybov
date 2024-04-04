using System;
using System.Collections;
using Snapping;
using Units.Movement;
using Units.Movement.Cat;
using Units.Movement.Shared;
using Units.Spawning;
using UnityEngine;

namespace Units
{
    public class Cat : MonoBehaviour, IFishThief, ISnappable, IStunnable
    {
        public StealableFish CarriedFish { get; private set; }
        
        [Header("IFishThief")]
        [SerializeField] private Transform _carryTransform;

        public TimeSpan StaggerDuration => TimeSpan.FromSeconds(_staggerDurationSeconds);
        [Header("IStunnable")]
        [SerializeField] private float _staggerDurationSeconds;

        public TimeSpan StunDuration => TimeSpan.FromSeconds(_stunDurationSeconds);
        [SerializeField] private float _stunDurationSeconds;


        [Header("ISnappable")]
        [SerializeField] private SnapConfig _snapConfig;

        [Header("Misc")]
        [SerializeField] private StateMachine _stateMachine;
        
        private IEnumerator _keepCarriedFishCloseRoutine;

        #region IFishThief

        public void OnFishSteal(StealableFish fish)
        {
            CarriedFish = fish;

            if (_keepCarriedFishCloseRoutine != null)
            {
                StopCoroutine(_keepCarriedFishCloseRoutine);
            }

            _keepCarriedFishCloseRoutine = KeepCarriedFishClose();
            StartCoroutine(_keepCarriedFishCloseRoutine);
        }

        public void OnFishDrop()
        {
            CarriedFish = null;

            if (_keepCarriedFishCloseRoutine != null)
            {
                StopCoroutine(_keepCarriedFishCloseRoutine);
            }
        }
        
        private IEnumerator KeepCarriedFishClose()
        {
            while (true)
            {
                CarriedFish.transform.position = _carryTransform.position;
                yield return null;
            }
            // ReSharper disable once IteratorNeverReturns
        }

        #endregion

        #region IOutOffBorderInteractable
        
        public void OnOutOfBorder()
        {
            if (CarriedFish == null)
            {
                return;
            }
            
            FishPool.CarryAwayFish(CarriedFish);
            
            Destroy(CarriedFish.gameObject);
            Destroy(gameObject);
        }

        #endregion
        
        #region ISnappable
        
        public void OnHit(float power)
        {
            HitType hitType = _snapConfig.GetHitType(power);
            SnapConfig.Handle(hitType, this);
        }

        public void GigaSnap()
        {
            _stateMachine.TryChangeState<KickedOutState>();
            
            if (CarriedFish == null)
            {
                return;
            }
            
            FishPool.DropFish(CarriedFish, this);
        }

        public void Snap()
        {
            Stun(StunDuration);

            if (CarriedFish == null)
            {
                return;
            }
            
            FishPool.DropFish(CarriedFish, this);
        }

        public void Slap()
        {
            Stun(StaggerDuration);
            
            if (CarriedFish == null)
            {
                return;
            }
            
            FishPool.DropFish(CarriedFish, this);
        }
        
        #endregion

        #region IStunnable
        
        public void Stun(TimeSpan duration)
        {
            StartCoroutine(StunRoutine(duration));
        }
        
        private IEnumerator StunRoutine(TimeSpan duration)
        {
            _stateMachine.TryChangeState<StunnedState>();
            yield return new WaitForSeconds((float) duration.TotalSeconds);
            _stateMachine.TryChangeState<ChaseForFishState>();
        }

        #endregion
    }
}