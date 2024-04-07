using UnityEngine;

namespace GlobalState.Level
{
    public class Level : MonoBehaviour
    {
        public static Level Instance { get; private set; }

        public uint NextLevelIndex => _nextLevelIndex;
        [SerializeField] private uint _nextLevelIndex;
        
        private void Awake()
        {
            if (Instance != null)
            {
                Destroy(gameObject);
                return;
            }
            
            Instance = this;
        }
    }
}