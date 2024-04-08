using System;
using System.Collections;
using Units.Hitting;
using Units.Movement;
using Units.Movement.Cat;
using Units.Movement.Shared;
using Units.Spawning;
using UnityEngine;

namespace Units
{
    public class Cat : Hittable, IFishThief, IStunnable
    {
        public override event Action<HitType> HitEvent;

        public event Action FishStoleEvent;
        public event Action FishDroppedEvent;
        
        public StealableFish CarriedFish { get; private set; }
        
        [Header("IFishThief")]
        [SerializeField] private Transform _carryTransform;

        public TimeSpan StaggerDuration => TimeSpan.FromSeconds(_staggerDurationSeconds);
        [Header("IStunnable")]
        [SerializeField] private float _staggerDurationSeconds;

        public TimeSpan StunDuration => TimeSpan.FromSeconds(_stunDurationSeconds);
        [SerializeField] private float _stunDurationSeconds;
        
        public HitConfig HitConfig => _hitConfig;
        [Header("ISnappable")]
        [SerializeField] private HitConfig _hitConfig;

        [Header("Misc")]
        [SerializeField] private StateMachine _stateMachine;
        
        private IEnumerator _keepCarriedFishCloseRoutine;
        private IEnumerator _stunRoutine;

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
            
            FishStoleEvent?.Invoke();
        }

        public void OnFishDrop()
        {
            CarriedFish = null;

            if (_keepCarriedFishCloseRoutine != null)
            {
                StopCoroutine(_keepCarriedFishCloseRoutine);
            }
            
            FishDroppedEvent?.Invoke();
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
            if (CarriedFish != null)
            {
                FishPool.CarryAwayFish(CarriedFish);
                Destroy(CarriedFish.gameObject);
            }
            
            Destroy(gameObject);
        }

        #endregion
        
        #region IHittable
        
        public override void OnHit(float power)
        {
            _hitConfig.Handle(power, this);
        }

        public override void GigaSnap()
        {
            HitEvent?.Invoke(HitType.GigaSnap);
            Hittable.StaticHitEvent?.Invoke(HitType.GigaSnap);
            _stateMachine.TryChangeState<KickedOutState>();
            
            if (_stunRoutine != null)
            {
                StopCoroutine(_stunRoutine);
            }
            
            if (CarriedFish == null)
            {
                return;
            }
            
            FishPool.DropFish(CarriedFish, this);
        }

        public override void Snap()
        {
            HitEvent?.Invoke(HitType.Snap);
            Hittable.StaticHitEvent?.Invoke(HitType.Snap);
            Stun(StunDuration);

            if (CarriedFish == null)
            {
                return;
            }
            
            FishPool.DropFish(CarriedFish, this);
        }

        public override void Slap()
        {
            HitEvent?.Invoke(HitType.Slap);
            Hittable.StaticHitEvent?.Invoke(HitType.Slap);
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
            if (_stunRoutine != null)
            {
                StopCoroutine(_stunRoutine);
            }
            
            _stunRoutine = StunRoutine(duration);
            StartCoroutine(_stunRoutine);
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