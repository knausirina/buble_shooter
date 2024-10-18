using System;
using Data;
using Field;
using UnityEngine;
using Views;

namespace GamePlay
{
    public class BubblesContact
    {
        public Action<BubbleView, BubblePosition> BubbleShoot;
        
        private BubbleView[,] _bubblesViews;
        private BubbleView _targetBubbleView;
        private FieldBuilder _fieldBuilder;
        private float _ballSize;

        private const float Offset = 0.1f;
        
        public void SetData(FieldBuilder fieldBuilder, BubbleView[,] bubblesViews, float ballSize)
        {
            _fieldBuilder = fieldBuilder;
            _bubblesViews = bubblesViews;
            _ballSize = ballSize;
        }

        public void SetTarget(BubbleView bubbleView)
        {
            _targetBubbleView = bubbleView;
        }

        public void CheckContact(bool isBroke)
        {
            if (_targetBubbleView == null)
            {
                return;
            }

            var find = false;
            for (var i = _bubblesViews.GetLength(0) - 1; i >= 0; i--)
            {
                for (var j = 0; j < _bubblesViews.GetLength(1); j++)
                {
                    var currentBubble = _bubblesViews[i, j];
                    if (currentBubble == null)
                    {
                        continue;
                    }

                    var pos1 = currentBubble.transform.position;
                    var pos2 = _targetBubbleView.transform.position;

                    var v1 = (Vector2)currentBubble.transform.position;
                    var v2 = (Vector2)_targetBubbleView.transform.position;
                    
                    var d = (v1 - v2).magnitude;
                    var dd = Vector2.Distance(v1, v2);
                    Debug.Log($"xxx v1 = {v1} v2 = {v2} pos1 = {pos1} pos2 = {pos2} dd = {dd} d = {d}");
                    Debug.Log("xxx d " + d + " i= " + i  + " j =  " + j + " _ballSize= " + _ballSize + " _ballSize * 2 = " + _ballSize * 2) ;
                    if (d < (_ballSize))
                    {
                        currentBubble.name = "hit";

                        if (!isBroke)
                        {
                            var row = i + 1;
                            
                            var d1 = (_fieldBuilder.GetPosition(row, j) - (Vector2)_targetBubbleView.transform.position).magnitude;
                            var d2 = (_fieldBuilder.GetPosition(row, j + 1) - (Vector2)_targetBubbleView.transform.position).magnitude;
                           

                            var column = (d1 < d2 ? j : j + 1);

                            Debug.Log("xxx d1 =  " + d1 + " d2 = " + d2 + " row "+ row + " column=" + column);
                          //  throw new UnityException();
                            _fieldBuilder.AddBubble(_targetBubbleView, row, column);
                            find = true;
                            _bubblesViews[row, column] = _targetBubbleView;
                            BubbleShoot?.Invoke(currentBubble, new BubblePosition(row, column));
                            break;
                        }
                        else
                        {
                            _fieldBuilder.RemoveBubble(currentBubble, i, j);
                            _fieldBuilder.AddBubble(_targetBubbleView, i, j);
                            _bubblesViews[i, j] = _targetBubbleView;
                        }
                    }
                }

                if (find)
                    break;
            }
        }
    }
}