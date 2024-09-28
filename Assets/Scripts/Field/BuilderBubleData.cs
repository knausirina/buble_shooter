using System;
using System.Collections.Generic;
using Data;
using UnityEngine;

namespace Field
{
    public class BuilderBubleData
    {
        public BubleData[][] GetData(string text)
        {
            var rows = text.Split(
                new string[] { Environment.NewLine },
                StringSplitOptions.None
            );
            Debug.Log("xxx rows .legnt =" + rows.Length);
            var result = new BubleData[rows.Length][];
            
            BubleData[] dataInRow = null;
            for (var i = 0; i < rows.Length; i++)
            {
                var row = rows[i];
                dataInRow = new BubleData[row.Length];
                Debug.Log("xxx row " + row);
                for (var j = 0; j < row.Length; j++)
                {
                    var symbol = row[j];
                    Debug.Log("xxx symbol = " + symbol + " (ColorEnum)symbol=" + (ColorEnum)symbol);
                    dataInRow[j] = new BubleData((ColorEnum)symbol, new Vector2Int(i, j));
                }

                result[i] = dataInRow;
            }
            Debug.Log(result.ToString());
            Debug.Log("xxx ");
            for (var i = 0; i < result.Length; i++)
            {
                for (var j = 0; j < result[i].Length; j++)
                {
                    Debug.Log($"xxxxxxx {i},{j}" +result[i][j].ToString());
                }
            }

            return result;
        }
    }
}