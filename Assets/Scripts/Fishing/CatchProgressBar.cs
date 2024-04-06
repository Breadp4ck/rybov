using System;
using System.Collections;
using System.Collections.Generic;
using Fishing.Handlers;
using Fishing.Pool;
using UnityEngine;

namespace UI.Fishing
{
    public class CatchProgressBar : MonoBehaviour
    {
        [SerializeField] private FishLake _fishLake;
        
        [Header("Slider")]
        [SerializeField] private Transform _minSide;
        [SerializeField] private Transform _maxSide;

        [SerializeField] private Transform _handleTransform;

        [SerializeField] private SpriteRenderer _background;
        [SerializeField] private SpriteRenderer _floaterSprite;

        private IEnumerator _displayProgressRoutine;
            
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
            _background.enabled = true;
            _floaterSprite.enabled = true;
            
            if (_displayProgressRoutine != null)
            {
                StopCoroutine(_displayProgressRoutine);
            }
            
            _displayProgressRoutine = DisplayProgress();
            StartCoroutine(_displayProgressRoutine);
        }

        private void OnEndCatching(CatchHandler.CatchResult obj)
        {
            _background.enabled = false;
            _floaterSprite.enabled = false;
            
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