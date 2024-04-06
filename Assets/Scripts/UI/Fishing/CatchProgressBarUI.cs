using System.Collections;
using Fishing.Handlers;
using Fishing.Pool;
using UnityEngine;

namespace UI.Fishing
{
    [RequireComponent(typeof(CanvasGroup))]
    public class CatchProgressBarUI : MonoBehaviour
    {
        [SerializeField] private FishLake _fishLake;
        
        [Header("Slider")]
        [SerializeField] private Transform _minSide;
        [SerializeField] private Transform _maxSide;

        [SerializeField] private Transform _handleTransform;

        private CanvasGroup _canvasGroup;

        private IEnumerator _displayProgressRoutine;

        private void Awake()
        {
            _canvasGroup = GetComponent<CanvasGroup>();
        }

        private void OnEnable()
        {
            _fishLake.StartCatchingEvent += OnStartCatching;
            _fishLake.EndCatchingEvent += OnEndCatching;
        }

        private void OnDisable()
        {
            _fishLake.StartCatchingEvent -= OnStartCatching;
            _fishLake.EndCatchingEvent -= OnEndCatching;
        }
        
        private void OnStartCatching()
        {
            _canvasGroup.alpha = 1f;
            
            if (_displayProgressRoutine != null)
            {
                StopCoroutine(_displayProgressRoutine);
            }
            
            _displayProgressRoutine = DisplayProgress();
            StartCoroutine(_displayProgressRoutine);
        }

        private void OnEndCatching(CatchHandler.CatchResult obj)
        {
            _canvasGroup.alpha = 0f;
            
            if (_displayProgressRoutine != null)
            {
                StopCoroutine(_displayProgressRoutine);
            }
        }
        
        private IEnumerator DisplayProgress()
        {
            while (true)
            {
                _handleTransform.position = Vector2.Lerp(_minSide.position, _maxSide.position, _fishLake.CatchHandler.CurrentCatchExtent / _fishLake.CatchHandler.MaxCatchExtent);
                yield return null;
            }
            // ReSharper disable once IteratorNeverReturns
        }
    }
}