using UnityEngine;

namespace RON.Scripts {
    public static class RectExtensions {
        public static Rect Inset(this Rect r, float left, float right, float top, float bottom)
            => Rect.MinMaxRect(r.xMin + left, r.yMin + bottom, r.xMax - right, r.yMax - top);
    }
}