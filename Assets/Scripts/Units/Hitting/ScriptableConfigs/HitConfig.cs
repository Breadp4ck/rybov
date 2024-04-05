using UnityEngine;

namespace Units.Hitting
{
    [CreateAssetMenu(fileName = "HitConfig", menuName = "Hitting/Config")]
    public class HitConfig : ScriptableObject
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
        
        public void Handle(float power, IHittable hittable)
        {
            switch (GetHitType(power))
            {
                case HitType.Slap:
                    hittable.Slap();
                    break;
                case HitType.Snap:
                    hittable.Snap();
                    break;
                case HitType.GigaSnap:
                    hittable.GigaSnap();
                    break;
            }
        }
        
        private HitType GetHitType(float power)
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
