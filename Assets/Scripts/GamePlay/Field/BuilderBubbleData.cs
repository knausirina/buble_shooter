using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class BuilderBubbleData
{
    private const char EmptySymbol = '0';
    private const int NumRowWithCountsBubbles = 0;
    private const int NumRowWithSizeField = 1;
    private const int NumRowWithMaxCountBubbles = 2;

    private readonly Config _config;

    public BuilderBubbleData(Config config)
    {
        _config = config;
    }

    public BubblesData GetData(string text, ref GameParameters gameParameters)
    {
        using (var strReader = new StringReader(text))
        {
            var numRow = 0;
            var result = new List<List<BubbleData>>();
            string line;
            while ((line = strReader.ReadLine()) != null)
            {
             //   Debug.Log($" line = {line} numRow = {numRow}");

                switch (numRow)
                {
                    case NumRowWithCountsBubbles:
                        {
                            var values = line.Split(' ');
                            var width = int.Parse(values[0]);
                            var height = int.Parse(values[1]);
                            gameParameters.FieldSizeInPixels = new Vector2Int(width, height);
                            break;
                        }
                    case NumRowWithSizeField:
                        {
                            var values = line.Split(' ');
                            var columns = int.Parse(values[0]);
                            var rows = int.Parse(values[1]);
                            gameParameters.FieldSizeInElements = new Vector2Int(rows, columns);
                            break;
                        }
                    case NumRowWithMaxCountBubbles:
                        gameParameters.MaxCountBubbles = int.Parse(line);
                        break;
                    default:
                        {
                            var dataInRow = new List<BubbleData>();
                            for (var j = 0; j < line.Length; j++)
                            {
                                var symbol = line[j];
                                if (symbol != EmptySymbol)
                                {
                                    var color = _config.GetColor(symbol);
                                    dataInRow.Add(new BubbleData(color, new Vector2Int(numRow, j)));
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