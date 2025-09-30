using UnityEditor;
using UnityEngine;

namespace RON.Scripts {
    public class BoardLayoutViewer : MonoBehaviour {
        public BoardLayoutConfiguration _config;
        public Color _faceColor = Color.green;
        public Color _outlineColor = Color.red;

        private void OnDrawGizmosSelected() {
            if (_config == null) {
                return;
            }

            var worldRect = LayoutUtils.CreateWorldRect(Camera.main, _config.AnchorMin, _config.AnchorMax);

            Handles.DrawSolidRectangleWithOutline(worldRect, _faceColor, _outlineColor);
        }
    }
}