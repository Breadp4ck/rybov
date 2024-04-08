using System;
using System.Collections;
using System.Collections.Generic;
using Fishing;
using UnityEngine;

public class FishingLine : MonoBehaviour
{
    [SerializeField] private Transform _fishingRod;
    [SerializeField] private Transform _fishPosition;

    private LineRenderer _lineRenderer;

    private void Awake()
    {
        _lineRenderer = GetComponent<LineRenderer>();
    }

    private void Update()
    {
        _lineRenderer.SetPosition(0, _fishingRod.transform.position);
        _lineRenderer.SetPosition(1, _fishPosition.transform.position);
    }
}
