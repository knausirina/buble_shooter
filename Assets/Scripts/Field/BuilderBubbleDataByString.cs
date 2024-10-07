using System.Collections.Generic;
using System.IO;
using Data;
using UnityEngine;

namespace Field
{
    public class BuilderBubbleDataByString
    {
        private readonly char _emptySymbol = '0';
        private const int NUM_ROW_WITH_COUNTS_BUBBLES = 0;
        private const int NUM_ROW_WITH_SIZE_FIELD = 1;
        
        public BubblesData GetData(string text, out Vector2Int fieldSizeInPixel, out Vector2Int fieldSizeInElements)
        {
            fieldSizeInPixel = Vector2Int.zero;
            fieldSizeInElements = Vector2Int.zero;
            
            using (var strReader = new StringReader(text))
            {
                string line;
                var i = 0;
                var result = new List<List<BubbleData>>();
                while ((line = strReader.ReadLine()) != null) 
                { 
                    Debug.Log($" line = {line}");

                    switch (i)
                    {
                        case NUM_ROW_WITH_COUNTS_BUBBLES:
                        {
                            var values = line.Split(' ');
                            var width = int.Parse(values[0]);
                            var height = int.Parse(values[1]);
                            fieldSizeInPixel = new Vector2Int(width, height);
                            break;
                        }
                        case NUM_ROW_WITH_SIZE_FIELD:
                        {
                            var values = line.Split(' ');
                            var columns = int.Parse(values[0]);
                            var rows = int.Parse(values[1]);
                            fieldSizeInElements = new Vector2Int(rows, columns);
                            break;
                        }
                        default:
                        {
                            var dataInRow = new List<BubbleData>();
                            for (var j = 0; j < line.Length; j++)
                            {
                                var symbol = line[j];
                                if (symbol != _emptySymbol)
                                {
                                    ColorEnum colorEnum = ColorEnum.Blue;
                                    if (!MapColors.GetColorEnum(symbol, out colorEnum))
                                    {
                                        Debug.LogWarning($"Incorrect color = {symbol}");
                                    }

                                    dataInRow.Add(new BubbleData(colorEnum, new Vector2Int(i, j)));
                                }
                                else
                                {
                                    dataInRow.Add(null);
                                }
                            }
                        
                            result.Add(dataInRow);
                            break;
                        }
                    }

                    i++;
                }
                return new BubblesData(result);
            }
        }
    }
}