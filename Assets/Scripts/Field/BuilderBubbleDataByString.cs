﻿using System.Collections.Generic;
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
        private const int NumRowWithMaxCountBubbles = 2;

        private readonly Config _config;

        public BuilderBubbleDataByString(Config config)
        {
            _config = config;
        }

        public BubblesData GetData(string text, out Vector2Int fieldSizeInPixel, out Vector2Int fieldSizeInElements, out int maxCountBubbles)
        {
            fieldSizeInPixel = Vector2Int.zero;
            fieldSizeInElements = Vector2Int.zero;
            maxCountBubbles  = 0;

            using (var strReader = new StringReader(text))
            {
                var numRow = 0;
                var result = new List<List<BubbleData>>();
                string line;
                while ((line = strReader.ReadLine()) != null) 
                { 
                    Debug.Log($" line = {line} numRow = {numRow}");

                    switch (numRow)
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
                        case NumRowWithMaxCountBubbles:
                            maxCountBubbles = int.Parse(line);
                            break;
                        default:
                        {
                            var dataInRow = new List<BubbleData>();
                            for (var j = 0; j < line.Length; j++)
                            {
                                var symbol = line[j];
                                if (symbol != EmptySymbol)
                                {
                                    var colorEnum = _config.GetColorByChar(symbol);
                                    dataInRow.Add(new BubbleData(colorEnum, new Vector2Int(numRow, j)));
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

                    numRow++;
                }
                return new BubblesData(result);
            }
        }
    }
}