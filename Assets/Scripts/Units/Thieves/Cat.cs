using System.Collections;
using Units.Spawning;
using UnityEngine;

namespace Units
{
    public class Cat : MonoBehaviour, IFishThief
    {
        public StealableFish CarriedFish { get; private set; }

        [SerializeField] private Transform _carryTransform;

        private IEnumerator _keepCarriedFishCloseRoutine;
        
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
    }
}