using System;
using UnityEngine;

namespace Units.Dragging
{
    public interface IDraggable
    {
        void StartDrag(Transform followTransform);
        void StopDrag();
    }
}