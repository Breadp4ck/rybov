using System.Collections;
using Fishing.Handlers;
using UnityEngine;

namespace Fishing
{
    [RequireComponent(typeof(IFishLake))]
    public class FishingRod : MonoBehaviour
    {
        public static FishingRod CurrentCatcher { get; private set; }
        
        private IFishLake _fishLake;
        
        private void Awake()
        {
            _fishLake = GetComponentInParent<IFishLake>();
        }
        
        public void StartCatching()
        {
            if (_fishLake.IsCatching == true)
            {
                return;
            }

            CurrentCatcher = this;
            _fishLake.StartCatching();
        }

        public void InterruptCatching()
        {
            if (_fishLake.IsCatching == false)
            {
                return;
            }

            CurrentCatcher = null;
            _fishLake.StopCatching(CatchHandler.CatchResult.Fail);
        }
    }
}
