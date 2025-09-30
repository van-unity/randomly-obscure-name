using System;
using UnityEngine;
using Object = UnityEngine.Object;

namespace RON.Scripts {
    public class TileContainer : IDisposable {
        private readonly int _columns;
        private readonly int _rows;
        private readonly GridLayoutCache _layout;
        private readonly Transform _container;

        private int _nextIndex;
        private bool _isDisposed;
        
        public Rect WorldRect => _layout.worldRect;

        public TileContainer(Camera camera, BoardLayoutConfiguration config, int columns, int rows, Bounds tileBounds) {
            _columns = columns;
            _rows = rows;
            _layout = GridLayoutCache.Create(camera, config, columns, rows, tileBounds);
            _container = new GameObject("TILE_CONTAINER").transform;
        }

        public void AddTile(Transform tileTransform) {
            if (!tileTransform) {
                throw new ArgumentException("tileTransform must be assigned");
            }

            if (_nextIndex >= _columns * _rows) {
                throw new InvalidOperationException("TileContainer is full.");
            }

            var col = _nextIndex % _columns;
            var row = _nextIndex / _columns;
            _nextIndex++;

            PlaceAndScale(tileTransform, col, row);
        }


        public void InsertTile(Transform tileTransform, int column, int row) {
            if (!tileTransform) {
                throw new ArgumentException("tileTransform must be assigned");
            }

            if (column < 0 || column >= _columns) {
                throw new ArgumentOutOfRangeException(nameof(column));
            }

            if (row < 0 || row >= _rows) {
                throw new ArgumentOutOfRangeException(nameof(row));
            }

            PlaceAndScale(tileTransform, column, row);
        }

        public bool TryGetBoardPosition(Vector3 clickPos, out BoardPosition boardPosition) {
            if (!_layout.contentRect.Contains(clickPos)) {
                boardPosition = default;
                return false;
            }

            var localX = clickPos.x - _layout.startX;
            var localY = clickPos.y - _layout.startY;

            var col = Mathf.FloorToInt(localX / (_layout.cellW + _layout.spacingX));
            var row = Mathf.RoundToInt(localY / (_layout.cellH + _layout.spacingY));

            if (col < 0 || col >= _columns || row < 0 || row >= _rows) {
                boardPosition = default;
                return false;
            }

            boardPosition = new BoardPosition(col, row);
            return true;
        }

        private void PlaceAndScale(Transform tile, int col, int row) {
            tile.SetParent(_container, true);
            var cx = _layout.startX + col * (_layout.cellW + _layout.spacingX) +
                     _layout.cellW * .5f;
            var cy = _layout.startY + row * (_layout.cellH + _layout.spacingY) +
                     _layout.cellH * .5f;
            tile.position = new Vector3(cx, cy, tile.position.z);

            tile.localScale = Vector3.one * _layout.scaleK;
        }

        public void Dispose() {
            if (_isDisposed) {
                return;
            }
#if UNITY_EDITOR
            if (Application.isPlaying) {
                Object.Destroy(_container.gameObject);
            } else {
                Object.DestroyImmediate(_container.gameObject);
            }
#else
            Object.Destroy(_container.gameObject);
#endif

            _isDisposed = true;
        }
    }

    public class TileContainerFactory {
        private readonly Camera _camera;
        private readonly BoardLayoutConfiguration _config;

        public TileContainerFactory(Camera camera, BoardLayoutConfiguration config) {
            _camera = camera;
            _config = config;
        }

        public TileContainer Create(int columns, int rows, Bounds tileBounds) =>
            new(_camera, _config, columns, rows, tileBounds);
    }
}