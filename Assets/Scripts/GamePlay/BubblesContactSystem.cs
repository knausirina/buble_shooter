using System;
using Field;
using UnityEngine;
using Views;

namespace GamePlay
{
    public class BubblesContactSystem
    {
        public Action<BubbleView> BubbleShoot;
        
        private BubbleView _targetBubbleView;
        private FieldBuilder _fieldBuilder;

        private const float Offset = 0.1f;
        
        public void SetData(FieldBuilder fieldBuilder)
        {
            _fieldBuilder = fieldBuilder;
        }

        public void SetTarget(BubbleView bubbleView)
        {
            _targetBubbleView = bubbleView;
        }

        private int hitNum = 0;
        
        public void CheckContact(bool isBroke)
        {
            if (_targetBubbleView == null)
            {
                return;
            }

            var find = false;
            for (var i = _fieldBuilder.BubblesViews.GetLength(0) - 1; i >= 0; i--)
            {
                for (var j = 0; j < _fieldBuilder.BubblesViews.GetLength(1); j++)
                {
                    var currentBubble = _fieldBuilder.BubblesViews[i, j];
                    if (currentBubble == null)
                    {
                        continue;
                    }

                    var v1 = (Vector2)currentBubble.transform.position;
                    var targetPositionVector2 = (Vector2)_targetBubbleView.transform.position;
                    
                    var d = Vector2.Distance(v1, targetPositionVector2);
                    if (d < _fieldBuilder.BallSize)
                    {
                        currentBubble.name = "hit" + hitNum++;

                        if (!isBroke)
                        {
                            var row = i + 1;
                            var currentColumnt = j;

                            if (i % 2 == 0)
                            {
                                currentColumnt++;
                            }
                            else
                            {
                                currentColumnt++;
                            }

                            var d1 = float.MaxValue;
                            if (_fieldBuilder.IsHasPosition(row, currentColumnt) && _fieldBuilder.BubblesViews[row, currentColumnt] == null)
                            {
                                d1 = (_fieldBuilder.GetPosition(row, currentColumnt) - targetPositionVector2).magnitude;
                            }

                            var d2 = float.MaxValue;
                            if (_fieldBuilder.IsHasPosition(row, currentColumnt + 1) && _fieldBuilder.BubblesViews[row, currentColumnt + 1] == null)
                            {
                                d2 = (_fieldBuilder.GetPosition(row, currentColumnt + 1) - targetPositionVector2).magnitude;
                            }

                            var d3 = float.MaxValue;
                            if (_fieldBuilder.IsHasPosition(row, currentColumnt - 1) && _fieldBuilder.BubblesViews[row, currentColumnt - 1] == null)
                            {
                                d3 = (_fieldBuilder.GetPosition(row, currentColumnt - 1) - targetPositionVector2).magnitude;
                            }

                            var targetColumn = currentColumnt;
                            var minD = d1;
                            if (d2 < d1)
                            {
                                targetColumn = currentColumnt + 1;
                                minD = d2;
                            }

                            if (d3 < minD)
                            {
                                targetColumn = currentColumnt - 1;
                            }

                            Debug.Log($"xxx d1 = {d1}  d2 = {d2} d3={d3} row ={row} column={targetColumn}");
                            _fieldBuilder.AddBubble(_targetBubbleView, row, targetColumn);
                            find = true;
                            _fieldBuilder.BubblesViews[row, targetColumn] = _targetBubbleView;
                            BubbleShoot?.Invoke(currentBubble);
                            break;
                        }
                        else
                        {
                            _fieldBuilder.RemoveBubble(currentBubble, i, j);
                            _fieldBuilder.AddBubble(_targetBubbleView, i, j);
                            _fieldBuilder.BubblesViews[i, j] = _targetBubbleView;
                        }
                    }
                }

                if (find)
                    break;
            }
        }
    }
}