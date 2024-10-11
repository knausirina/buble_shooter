using UnityEngine;

namespace Field
{
    public class FieldDecoration
    {
        public void Build(Game game)
        {
            var gameContext = game.GameContext;
            
            gameContext.FieldRectTransform.sizeDelta = new Vector2(game.FieldSizeInPixels.x, game.FieldSizeInPixels.y);
        }
    }
}