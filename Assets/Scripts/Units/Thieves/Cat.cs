using System.Collections;
using UnityEngine;

namespace Units
{
    public class Cat : MonoBehaviour, IFishThief
    {
        public GameObject GameObject => gameObject;
        
        public StealableFish CarriedStealableFish { get; private set; }

        [SerializeField] private Vector2 _carryPositionOffset;

        private IEnumerator _keepCarriedFishCloseRoutine;
        
        private void Update()
        {
            if (CarriedStealableFish == null)
            {
                return;
            }
            
            CarriedStealableFish.transform.position = (Vector2) transform.position + _carryPositionOffset;
        }
        
        public void Steal(StealableFish fish)
        {
            CarriedStealableFish = fish;

            if (_keepCarriedFishCloseRoutine != null)
            {
                StopCoroutine(_keepCarriedFishCloseRoutine);
            }

            _keepCarriedFishCloseRoutine = KeepCarriedFishClose();
            StartCoroutine(_keepCarriedFishCloseRoutine);
        }

        public void Drop(StealableFish fish)
        {
            CarriedStealableFish = null;

            if (_keepCarriedFishCloseRoutine != null)
            {
                StopCoroutine(_keepCarriedFishCloseRoutine);
            }
        }
        
        private IEnumerator KeepCarriedFishClose()
        {
            while (true)
            {
                CarriedStealableFish.transform.position = (Vector2) transform.position + _carryPositionOffset;
                yield return null;
            }
            // ReSharper disable once IteratorNeverReturns
        }
    }
}