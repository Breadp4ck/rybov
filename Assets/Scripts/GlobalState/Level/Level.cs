using UnityEngine;

namespace GlobalState.Level
{
    public class Level : MonoBehaviour
    {
        public static Level Instance { get; private set; }

        public Constants.SceneType NextSceneType => _nextNextSceneType;
        [SerializeField] private Constants.SceneType _nextNextSceneType;
        
        public Constants.SceneType CurrentSceneType => _currentSceneType;
        [SerializeField] private Constants.SceneType _currentSceneType;
        
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