using UnityEngine;

namespace Snapping
{
    [CreateAssetMenu(fileName = "SnapConfig", menuName = "Snapping/Config")]
    public class SnapConfig : ScriptableObject
    {
        /// <summary>
        /// Lowest possible hit
        /// The minimum power required to snap the object
        /// </summary>
        [SerializeField] private float _slapThreshold;
        
        /// <summary>
        /// Last possible hit before kick out the game field
        /// </summary>
        [SerializeField] private float _snapThreshold;
        
        /// <summary>
        /// Kick out the game field
        /// </summary>
        [SerializeField] private float _gigaSnapThreshold;

        public static void Handle(HitType hitType, ISnappable snappable)
        {
            switch (hitType)
            {
                case HitType.Slap:
                    snappable.Slap();
                    break;
                case HitType.Snap:
                    snappable.Snap();
                    break;
                case HitType.GigaSnap:
                    snappable.GigaSnap();
                    break;
            }
        }

        public HitType GetHitType(float power)
        {
            if (power >= _gigaSnapThreshold)
            {
                return HitType.GigaSnap;
            }
            
            if (power >= _snapThreshold)
            {
                return HitType.Snap;
            }

            if (power >= _slapThreshold)
            {
                return HitType.Slap;
            }

            return HitType.None;
        }
    }
}
