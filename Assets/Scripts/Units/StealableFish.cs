using UnityEngine;

namespace Units
{
    /// <summary>
    /// Fish that registers in FishPool.cs
    /// </summary>
    public class StealableFish : MonoBehaviour
    {
        /// <summary>
        /// The one who stole and carries this fish at the moment.
        /// </summary>
        public IFishThief Thief;
    }
}