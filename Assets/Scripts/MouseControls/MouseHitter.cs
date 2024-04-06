using System;
using System.Linq;
using Inputs;
using Units.Hitting;
using UnityEngine;
using Zenject;

namespace MouseControls
{
    public class MouseHitter : MonoBehaviour
    {
        public event Action StartChargeEvent;
        public event Action StopChargeEvent;
        
        [Header("Range")] [Range(0.1f, 2f)] [SerializeField]
        private float _radius;

        /// <summary>
        /// How long the player should hold down some button to accumulate max power (1f).
        /// </summary>
        [Header("Power")] [Range(0.25f, 5f)] [SerializeField]
        private float _maxPowerTimeSeconds;

        public float MaxPower => _maxPower;
        [Range(1f, 5f)] 
        [SerializeField] private float _maxPower;
        
        public float CurrentPower { get; private set; }

        private IInputSystem _inputSystem;

        /// <summary>
        /// If the player is holding down some button.
        /// </summary>
        private bool _isAccumulatingPower;

        [Inject]
        private void Construct(IInputSystem inputSystem)
        {
            _inputSystem = inputSystem;
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, _radius);
        }

        private void Update()
        {
            if (_inputSystem.IsActionDown(InputAction.LeftClick) == true)
            {
                StartCharge();
                return;
            }

            if (_inputSystem.IsActionPressed(InputAction.LeftClick) == true && _isAccumulatingPower == true)
            {
                Charge(Time.deltaTime / _maxPowerTimeSeconds);
            }

            if (_inputSystem.IsActionUp(InputAction.LeftClick) == true && _isAccumulatingPower == true)
            {
                StopCharge(CurrentPower);
            }
        }

        private void StartCharge()
        {
            _isAccumulatingPower = true;
            CurrentPower = 0f;
            
            StartChargeEvent?.Invoke();
        }

        private void Charge(float deltaPower)
        {
            if (CurrentPower >= _maxPower)
            {
                return;
            }

            CurrentPower += deltaPower;
            CurrentPower = Mathf.Clamp(CurrentPower, 0f, _maxPower);
        }

        private void StopCharge(float power)
        {
            Hit(power);
            
            StopChargeEvent?.Invoke();
        }
        
        private void Hit(float power)
        {
            _isAccumulatingPower = false;

            power = Mathf.Clamp(power, 0f, _maxPower);

            Collider2D[] overlappedColliders = new Collider2D[4];
            int count = Physics2D.OverlapCircleNonAlloc(transform.position, _radius, overlappedColliders);

            if (count == 0)
            {
                return;
            }
            
            overlappedColliders = overlappedColliders.Where(x => x != null).ToArray();
            
            foreach (Collider2D overlapped in overlappedColliders)
            {
                if (overlapped.TryGetComponent(out IHittable hittable) == false)
                {
                    continue;
                }

                hittable.OnHit(power);
            }
        }
    }
}