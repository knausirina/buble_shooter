using UnityEngine;

namespace Data
{
    public class BublesData
    {
        private readonly BubleData[][] _data;

        public BublesData(BubleData[][] data)
        {
            _data = data;
        }

        public BubleData Get(int row, int column)
        {
            return _data[row][column];
        }

        public int RowsCount => _data.Length;
        public int ColumnCount => _data[0]?.Length ?? 0;
    }
}