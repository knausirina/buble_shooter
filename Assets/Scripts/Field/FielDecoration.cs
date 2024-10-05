using Data;
using UnityEngine;

namespace Field
{
    public class FieldDecoration
    {
        public void Build(GameContext gameContext, BublesData bublesData)
        {
            gameContext.FieldRectTransform.sizeDelta =
                new Vector2(bublesData.FieldSizeInPixels.x, bublesData.FieldSizeInPixels.y);
            
            var canvasHeight = gameContext.Canvas.GetComponent<RectTransform>().rect.height;

            var worldPosition = gameContext.Camera.ScreenToWorldPoint(new Vector3(0,
                canvasHeight - gameContext.FieldRectTransform.sizeDelta.y +
                gameContext.FieldRectTransform.anchoredPosition.y, gameContext.Camera.nearClipPlane));
            
            worldPosition.x = 0;
            gameContext.Slingshot.gameObject.transform.position = worldPosition;

            var pos2 = new Vector3(worldPosition.x, worldPosition.y - 0.5f, worldPosition.z);
            gameContext.SlingshotLinesTransform.position = pos2;
        }
    }
}