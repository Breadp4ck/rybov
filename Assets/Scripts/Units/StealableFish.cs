using Units.Destroying;
using Units.Movement.Fish;
using Units.Spawning;
using UnityEngine;

namespace Units
{
    /// <summary>
    /// Fish that registers in FishPool.cs
    /// </summary>
    public class StealableFish : MonoBehaviour, IOutOfBorderInteractable
    {
        /// <summary>
        /// The one who stole and carries this fish at the moment.
        /// </summary>
        public IFishThief Thief;

        [SerializeField] private FishMovementStateMachine _stateMachine;
        
        public void OnSteal(IFishThief thief)
        {
            Thief = thief;
            _stateMachine.TryChangeState<CarriedState>();
        }
        
        public void OnDrop()
        {
            Thief = null;
            _stateMachine.TryChangeState<FidgetingCooldownState>();
        }

        public void OnOutOfBorder()
        {
            // Let thief handle out of border.
            if (Thief != null)
            {
                return;
            }
            
            FishPool.OutOfBorder(this);
            Destroy(gameObject);
        }
    }
}