using UnityEngine;

namespace RON.Scripts {
    public readonly struct GridLayoutCache {
        public readonly Rect worldRect;
        public readonly Rect contentRect;
        public readonly float spacingX;
        public readonly float spacingY;
        public readonly float cellW;
        public readonly float cellH;
        public readonly float gridW;
        public readonly float gridH;
        public readonly float startX;
        public readonly float startY;
        public readonly float scaleK;

        private GridLayoutCache(Rect worldRect, Rect contentRect, float spacingX, float spacingY, float cellW,
            float cellH, float gridW, float gridH, float startX, float startY, float scaleK) {
            this.worldRect = worldRect;
            this.contentRect = contentRect;
            this.spacingX = spacingX;
            this.spacingY = spacingY;
            this.cellW = cellW;
            this.cellH = cellH;
            this.gridW = gridW;
            this.gridH = gridH;
            this.startX = startX;
            this.startY = startY;
            this.scaleK = scaleK;
        }

        public static GridLayoutCache Create(Camera camera, BoardLayoutConfiguration config, int cols, int rows,
            Bounds tileBounds) {
            var worldRect = LayoutUtils.CreateWorldRect(camera, config.AnchorMin, config.AnchorMax);
            var contentRect = worldRect.Inset(config.PaddingLeft, config.PaddingRight, config.PaddingTop,
                config.PaddingBottom);

            var spacingX = Mathf.Max(0, config.SpacingX);
            var spacingY = Mathf.Max(0, config.SpacingY);

            var cellW = (contentRect.width - spacingX * Mathf.Max(0, cols - 1)) / cols;
            var cellH = (contentRect.height - spacingY * Mathf.Max(0, rows - 1)) / rows;

            var gridW = cols * cellW + (cols - 1) * spacingX;
            var gridH = rows * cellH + (rows - 1) * spacingY;

            var startX = contentRect.center.x - gridW * .5f;
            var startY = contentRect.center.y - gridH * .5f;

            var k = Mathf.Min(cellW / tileBounds.size.x, cellH / tileBounds.size.y);

            return new GridLayoutCache(
                worldRect,
                contentRect,
                spacingX,
                spacingY,
                cellW,
                cellH,
                gridW,
                gridH,
                startX,
                startY,
                k
            );
        }
    }
}