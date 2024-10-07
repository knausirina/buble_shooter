﻿using UnityEngine;

namespace Views
{
    public class BubbleView : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer _renderer = default;
        
        public SpriteRenderer Renderer => _renderer;

        public void SetColor(Color color)
        {
            _renderer.color = color;
        }
    }
}