using System.Collections.Generic;

namespace Data
{
    public class BubblesData
    {
        private readonly List<List<BubbleData>> _data;

        public BubblesData(List<List<BubbleData>> data)
        {
            _data = data;
        }

        public BubbleData Get(int row, int column)
        {
            return _data[row][column];
        }

        public int RowsCount => _data.Count;
        public int ColumnCount => _data[0]?.Count ?? 0;
    }
}