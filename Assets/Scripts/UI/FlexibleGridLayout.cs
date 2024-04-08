using GlobalStates.Game;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace UI
{
    public class FlexibleGridLayout : LayoutGroup
    {
        public enum FitType
        {
            Uniform,
            Width,
            Height,
            FixedRows,
            FixedColumns
        }

        [Header("Flexible Grid")]
        [SerializeField] private FitType _fitType;

        [SerializeField] private int _rows;
        [SerializeField] private int _columns;
        private Vector2 _cellSize;
        [SerializeField] private Vector2 _spacing;

        public override void CalculateLayoutInputHorizontal()
        {
            base.CalculateLayoutInputHorizontal();
            float squareRoot = Mathf.Sqrt(transform.childCount);

            if (_fitType == FitType.Width || _fitType == FitType.Height || _fitType == FitType.Uniform)
            {
                _rows = _columns = Mathf.CeilToInt(squareRoot);
            }

            if (_fitType == FitType.Width || _fitType == FitType.FixedColumns)
            {
                _rows = Mathf.CeilToInt(transform.childCount / (float)_columns);
            }
            if (_fitType == FitType.Height || _fitType == FitType.FixedRows)
            {
                _columns = Mathf.CeilToInt(transform.childCount / (float)_rows);
            }

            float parentWidth = rectTransform.rect.width;
            float parentHeight = rectTransform.rect.height;

            float cellWidth = parentWidth / (float)_columns - ((_spacing.x / (float)_columns * 2))
                - (padding.left / (float)_columns) - (padding.right / (float)_columns);
            float cellHeight = parentHeight / (float)_rows - ((_spacing.y / (float)_rows) * 2)
                - (padding.top / (float)_rows) - (padding.bottom / (float)_rows); ;
            
            _cellSize.x = cellWidth;
            _cellSize.y = cellHeight;
            

            int columnCount = 0;
            int rowCount = 0;

            for (int i = 0; i < rectChildren.Count; i++)
            {
                rowCount = i / _columns;
                columnCount = i % _columns;

                var item = rectChildren[i];

                var xPos = (_cellSize.x * columnCount) + (_spacing.x * columnCount) + padding.left;
                var yPos = (_cellSize.y * rowCount) + (_spacing.y * rowCount) + padding.top;

                SetChildAlongAxis(item, 0, xPos, _cellSize.x);
                SetChildAlongAxis(item, 1, yPos, _cellSize.y);

            }
        }

        public override void CalculateLayoutInputVertical() { }

        public override void SetLayoutHorizontal() { }

        public override void SetLayoutVertical() { }
    }
}