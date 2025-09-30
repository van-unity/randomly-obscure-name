using UnityEngine;

namespace RON.Scripts {
    public static class LayoutUtils {
        public static Rect CreateWorldRect(Camera camera, Vector2 anchorMin, Vector2 anchorMax) {
            var bottomLeft = camera.ViewportToWorldPoint(anchorMin);
            var topRight = camera.ViewportToWorldPoint(anchorMax);
            
            return Rect.MinMaxRect(
                Mathf.Min(bottomLeft.x, topRight.x),
                Mathf.Min(bottomLeft.y, topRight.y),
                Mathf.Max(bottomLeft.x, topRight.x),
                Mathf.Max(bottomLeft.y, topRight.y)
            );
        }
    }
}