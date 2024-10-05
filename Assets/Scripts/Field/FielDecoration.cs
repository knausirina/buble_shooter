using Data;
using UnityEngine;

namespace Field
{
    public class FieldDecoration
    {
        private const float OFFSE_BALL_POSITION_Y = 0.5f;
        
        public void Build(GameContext gameContext, BubblesData bubblesData)
        {
            gameContext.FieldRectTransform.sizeDelta =
                new Vector2(bubblesData.FieldSizeInPixels.x, bubblesData.FieldSizeInPixels.y);
            
            var canvasHeight = gameContext.Canvas.GetComponent<RectTransform>().rect.height;

            var worldPosition = gameContext.Camera.ScreenToWorldPoint(new Vector3(0,
                canvasHeight - gameContext.FieldRectTransform.sizeDelta.y +
                gameContext.FieldRectTransform.anchoredPosition.y, gameContext.Camera.nearClipPlane));
            
            worldPosition.x = 0;
            worldPosition.y += OFFSE_BALL_POSITION_Y;
            gameContext.Slingshot.gameObject.transform.position = worldPosition;

            var pos2 = new Vector3(worldPosition.x, worldPosition.y, worldPosition.z);
            gameContext.SlingshotLinesTransform.position = pos2;
        }
    }
}