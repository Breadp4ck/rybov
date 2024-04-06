using System;
using System.Collections.Generic;
using Units;
using Units.Movement.Fish;
using UnityEngine;

namespace Fishing
{
    [RequireComponent(typeof(Collider2D), typeof(Rigidbody2D))]
    public class FishDestroyer : MonoBehaviour
    {
        private readonly List<StealableFish> _fishInside = new();
        
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.TryGetComponent(out StealableFish fish) == false)
            {
                return;
            }
            
            // :^( 
            if (fish.StateMachine.CurrentState is CarriedState)
            {
                fish.DragStoppedEvent += OnFishDragStopped;
                _fishInside.Add(fish);
                return;
            }
            
            fish.OnOutOfBorder();
        }
        
        private void OnTriggerExit2D(Collider2D other)
        {
            if (other.TryGetComponent(out StealableFish fish) == false)
            {
                return;
            }

            if (_fishInside.Contains(fish) == false)
            {
                return;
            }
            
            fish.DragStoppedEvent -= OnFishDragStopped;
            _fishInside.Remove(fish);
        }

        private void OnFishDragStopped(StealableFish fish)
        {
            if (_fishInside.Contains(fish) == false)
            {
                return;
            }
            
            fish.DragStoppedEvent -= OnFishDragStopped;
            fish.OnOutOfBorder();
        }
    }
}