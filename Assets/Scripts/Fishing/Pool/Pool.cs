using Fishing.Fish;
using Inputs;
using Unity.Mathematics;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;

namespace Fishing.Pool
{
    public class Pool : MonoBehaviour, IPool
    {
        [SerializeField] private Camera _camera;
        
        [SerializeField] private SpawnHandler _spawnHandler;
        
        /// <summary>
        /// How long player will wait to catch the fish.
        /// </summary>
        [Header("Fishing time")] [SerializeField] [Range(0.25f, 5f)]
        private float _minFishingTimeInSeconds;

        [SerializeField] [Range(1f, 5f)] 
        private float _maxFishingTimeInSeconds;

        /// <summary>
        /// If player trying to catch the fish.
        /// </summary>
        private bool _isInFishingState = false;
        
        /// <summary>
        /// Time for fish waiting.
        /// </summary>
        private float _fishingTime;

        private float _currentTime;

        private Vector3 _mouseWorldPosition;
        
        private IInputSystem _inputSystem;
        
        [Inject]
        private void Construct(IInputSystem inputSystem)
        {
            _inputSystem = inputSystem;
        }

        private void Update()
        {
            if (_inputSystem.IsActionDown(InputAction.LeftClick) && !_isInFishingState)
            {
                StartFishing();
                return;
            }

            if (_isInFishingState)
            {
                WaitingToCatchFish(Time.deltaTime);
            }

            if (_currentTime > _fishingTime)
            {
                _isInFishingState = false;
                _currentTime = 0f;
                
                _spawnHandler.SpawnFish();
            }
        }
        
        private void FixedUpdate()
        {
            transform.position = _camera.ScreenToWorldPoint(Input.mousePosition);
        }

        private void StartFishing()
        {
            _isInFishingState = true;
            _fishingTime = Random.Range(_minFishingTimeInSeconds, _maxFishingTimeInSeconds);

            _mouseWorldPosition = _camera.ScreenToWorldPoint(Input.mousePosition);
            _mouseWorldPosition.z = _camera.nearClipPlane;
        }

        private void WaitingToCatchFish(float deltaTime)
        {
            _currentTime += deltaTime;
        }

        private void GetFish(GameObject prefab)
        {
            Instantiate(prefab, _mouseWorldPosition, Quaternion.identity);
        }
    }
}