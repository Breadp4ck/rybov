using System;
using System.Collections;
using MouseControls;
using UnityEngine;

namespace UI.Hitting
{
    [RequireComponent(typeof(CanvasGroup), typeof(MouseFollower))]
    public class HitProgressBarUI : MonoBehaviour
    {
        [SerializeField] private MouseHitter _mouseHitter;
        
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
            _mouseHitter.StartChargeEvent += OnStartCharge;
            _mouseHitter.StopChargeEvent += OnStopCharge;
        }
        
        private void OnDisable()
        {
            _mouseHitter.StartChargeEvent -= OnStartCharge;
            _mouseHitter.StopChargeEvent -= OnStopCharge;
        }

        private void OnStartCharge()
        {
            _canvasGroup.alpha = 1f;
            
            if (_displayProgressRoutine != null)
            {
                StopCoroutine(_displayProgressRoutine);
            }
            
            _displayProgressRoutine = DisplayProgress();
            StartCoroutine(_displayProgressRoutine);
        }
        
        private void OnStopCharge()
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
                _handleTransform.position = Vector2.Lerp(_minSide.position, _maxSide.position, _mouseHitter.CurrentPower / _mouseHitter.MaxPower);
                yield return null;
            }
            // ReSharper disable once IteratorNeverReturns
        }
    }
}