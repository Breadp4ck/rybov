using System;
using System.Collections;
using System.Collections.Generic;
using MouseControls;
using UnityEngine;

public class CursorSpriteHandler : MonoBehaviour
{
    [SerializeField] private MouseHitter _hitter;
    [SerializeField] private MouseFishDragger _fishDragger;
    [SerializeField] private MouseFishing _fishing;
    
    [Header("Sprites")] 
    [SerializeField] private Texture2D _defaultSprite;
    [SerializeField] private Texture2D _hitSprite;
    [SerializeField] private Texture2D _fishDragSprite;
    [SerializeField] private Texture2D _fishingSprite;

    private void OnEnable()
    {
        _hitter.StartChargeEvent += OnStartCharge;
        _hitter.StopChargeEvent += OnStopCharge;
        
        _fishDragger.StartDragEvent += OnStartDrag;
        _fishDragger.StopDragEvent += OnStopDrag;
        
        _fishing.StartPullFishingRodEvent += OnStartFishing;
        _fishing.StopPullFishingRodEvent += OnStopFishing;
    }
    
    private void OnDisable()
    {
        _hitter.StartChargeEvent -= OnStartCharge;
        _hitter.StopChargeEvent -= OnStopCharge;
        
        _fishDragger.StartDragEvent -= OnStartDrag;
        _fishDragger.StopDragEvent -= OnStopDrag;
        
        _fishing.StartPullFishingRodEvent -= OnStartFishing;
        _fishing.StopPullFishingRodEvent -= OnStopFishing;
    }
    
    private void SetDefaultCursor()
    {
        Cursor.SetCursor(_defaultSprite, Vector2.zero, CursorMode.Auto);
    } 
    
    private void OnStartCharge()
    {
        Cursor.SetCursor(_hitSprite, Vector2.zero, CursorMode.Auto);
    }
    
    private void OnStopCharge()
    {
        SetDefaultCursor();
    }
    
    private void OnStartDrag()
    {
        Cursor.SetCursor(_fishDragSprite, Vector2.zero, CursorMode.Auto);
    }
    
    private void OnStopDrag()
    {
        SetDefaultCursor();
    }
    
    private void OnStartFishing()
    {
        Cursor.SetCursor(_fishingSprite, Vector2.zero, CursorMode.Auto);
    }
    
    private void OnStopFishing()
    {
        SetDefaultCursor();
    }
}
