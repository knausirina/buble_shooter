using System.Collections.Generic;
using System.IO;
using Data;
using UnityEngine;

namespace Field
{
    public class BuilderBubbleDataByString
    {
        private const char EmptySymbol = '0';
        private const int NumRowWithCountsBubbles = 0;
        private const int NumRowWithSizeField = 1;

        private readonly Config _config;

        public BuilderBubbleDataByString(Config config)
        {
            _config = config;
        }

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
                        case NumRowWithCountsBubbles:
                        {
                            var values = line.Split(' ');
                            var width = int.Parse(values[0]);
                            var height = int.Parse(values[1]);
                            fieldSizeInPixel = new Vector2Int(width, height);
                            break;
                        }
                        case NumRowWithSizeField:
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
                                if (symbol != EmptySymbol)
                                {
                                    var colorEnum = _config.GetColorByChar(symbol);
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