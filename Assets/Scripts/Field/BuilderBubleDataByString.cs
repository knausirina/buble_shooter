using System;
using Data;
using UnityEngine;

namespace Field
{
    public class BuilderBubleDataByString
    {
        public BublesData GetData(string text)
        {
            var rows = text.Split(
                new string[] { Environment.NewLine },
                StringSplitOptions.None
            );
            var result = new BubleData[rows.Length][];

            for (var i = 0; i < rows.Length; i++)
            {
                var row = rows[i];
                var dataInRow = new BubleData[row.Length];
                for (var j = 0; j < row.Length; j++)
                {
                    var symbol = row[j];
                    if (!int.TryParse(symbol.ToString(), out var intSymbol))
                    {
                        Debug.LogWarning($"Incorrect color with value = {symbol}");
                    }

                    var colorEnum = (ColorEnum)intSymbol;
                    if (colorEnum == ColorEnum.None)
                    {
                        dataInRow[j] = null;
                    }
                    else
                    {
                        dataInRow[j] = new BubleData(colorEnum, new Vector2Int(i, j));
                    }
                }

                result[i] = dataInRow;
            }
            
            return new BublesData(result);
        }
    }
}