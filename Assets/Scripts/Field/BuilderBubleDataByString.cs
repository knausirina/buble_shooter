using System.Collections.Generic;
using System.IO;
using Data;
using UnityEngine;

namespace Field
{
    public class BuilderBubleDataByString
    {
        private char _emptySymbol = '0';
        
        public BublesData GetData(string text)
        {
            var fieldSizeInPixel = Vector2Int.zero;
            var fieldSizeInElements = Vector2Int.zero;
            
            using (var strReader = new StringReader(text))
            {
                string line;
                var i = 0;
                var result = new List<List<BubleData>>();
                while ((line = strReader.ReadLine()) != null) 
                { 
                    Debug.Log($" line = {line}");

                    switch (i)
                    {
                        case 0:
                        {
                            var values = line.Split(' ');
                            var width = int.Parse(values[0]);
                            var height = int.Parse(values[1]);
                            fieldSizeInPixel = new Vector2Int(width, height);
                            break;
                        }
                        case 1:
                        {
                            var values = line.Split(' ');
                            var columns = int.Parse(values[0]);
                            var rows = int.Parse(values[1]);
                            fieldSizeInElements = new Vector2Int(rows, columns);
                            break;
                        }
                        default:
                        {
                            var dataInRow = new List<BubleData>();
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

                                    dataInRow.Add(new BubleData(colorEnum, new Vector2Int(i, j)));
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
                return new BublesData(result, fieldSizeInPixel, fieldSizeInElements);
            }
        }
    }
}