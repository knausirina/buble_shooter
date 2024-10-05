using System.Collections.Generic;
using UnityEngine;
namespace Data
{
    public class BubblesData
    {
        private readonly List<List<BubbleData>> _data;
        private readonly Vector2Int _fieldSizeInPixels;
        private readonly Vector2Int _fieldSizeInElements;

        public BubblesData(List<List<BubbleData>> data, Vector2Int fieldSizeInPixels, Vector2Int fieldSizeInElements)
        {
            _data = data;
            _fieldSizeInPixels = fieldSizeInPixels;
            _fieldSizeInElements = fieldSizeInElements;
        }

        public BubbleData Get(int row, int column)
        {
            return _data[row][column];
        }

        public int RowsCount => _data.Count;
        public int ColumnCount => _data[0]?.Count ?? 0;
        public Vector2Int FieldSizeInPixels => _fieldSizeInPixels;
        public Vector2Int FieldSizeInElements => _fieldSizeInElements;
    }
}