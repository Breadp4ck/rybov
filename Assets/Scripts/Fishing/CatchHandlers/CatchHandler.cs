using System;
using Fishing.Fish;
using UnityEngine;

namespace Fishing.Handlers
{
    public abstract class CatchHandler : MonoBehaviour
    {
        public enum CatchResult : byte
        {
            Fail,
            Success,
        }
        
        public abstract event Action<CatchResult> CatchFinishedEvent;
        
        /// <summary>
        /// From 0.0f to MaxCatchExtent.
        /// </summary>
        public abstract float CurrentCatchExtent { get; protected set; }
        
        public abstract float MaxCatchExtent { get; }

        public abstract void StartCatching(FishInfo fishInfo);
        public abstract void StopCatching();
    }
}