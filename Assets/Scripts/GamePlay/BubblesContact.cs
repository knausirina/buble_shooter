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

            // Debug.Log("xxx CheckContact!!!");
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

                    var d = (_bubblesViews[i, j].transform.position - _targetBubbleView.transform.position).sqrMagnitude;
                    //Debug.Log("xxx d " + d);
                    if (d < (_ballSize + Offset))
                    {
                        currentBubble.name = "hit";

                        if (!isBroke)
                        {
                            Debug.Log($"xxx i = {i} j = {j}");
                            _fieldBuilder.AddBubble(_targetBubbleView, i + 1, j);
                            find = true;
                            _bubblesViews[i + 1, j] = _targetBubbleView;
                            BubbleShoot?.Invoke(currentBubble, new BubblePosition(i, j));
                            break;
                        }
                        else
                        {
                            _fieldBuilder.RemoveBubble(currentBubble, i, j);
                            _fieldBuilder.AddBubble(_targetBubbleView, i, j);
                            _bubblesViews[i, j] = _targetBubbleView;
                        }
                        
                       // throw new Exception();
                    }
                }

                if (find)
                    break;
            }
        }
    }
}