using UnityEngine;
using Views;

public class ResultGameSystem
{
    private readonly Config _config;

    public ResultGameSystem(Config config)
    {
        _config = config;
    }

    public bool IsWin(BubbleView[,] bubbleViews, Vector2Int sizeFieldInElements)
    {
        var ballsInFirshRowCount = 0;
        for (var i = 0; i < bubbleViews.GetLength(1); i++)
        {
            if (bubbleViews[0, 1] != null)
            {
                ballsInFirshRowCount++;
            }
        }

        if (ballsInFirshRowCount > _config.ConditionWinInLastRowPercent * sizeFieldInElements.x)
        {
            return false;
        }

        for (var i = 1; i < bubbleViews.GetLength(1); i++)
        {
            for (var j = 0; j < bubbleViews.GetLength(1); j++)
            {
                if (bubbleViews[i, j] != null)
                {
                    return false;
                }
            }
        }

        return true;
    }
}