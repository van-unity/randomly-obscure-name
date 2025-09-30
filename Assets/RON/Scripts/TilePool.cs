using System;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

namespace RON.Scripts {
    public class TilePool : IDisposable {
        private readonly TileWidget _prefab;
        private readonly int _initialSize;
        private readonly int _growAmount;
        private readonly List<TileWidget> _pool;
        private readonly Transform _container;

        private bool _isDisposed;

        public TilePool(TileWidget prefab, int initialSize, int growAmount) {
            _prefab = prefab;
            _initialSize = initialSize;
            _growAmount = growAmount;
            _pool = new List<TileWidget>(_initialSize);
            _container = new GameObject("TILE_POOL").transform;
            AddTiles(_initialSize);
        }

        public TileWidget Get() {
            if (_pool.Count == 0) {
                AddTiles(_growAmount);
            }

            var tileWidget = _pool[0];
            _pool.RemoveAt(0);
            return tileWidget;
        }

        public void Return(TileWidget tile) {
            _pool.Add(tile);
            tile.Clear();
            tile.gameObject.SetActive(false);
        }

        private void AddTiles(int count) {
            for (int i = 0; i < count; i++) {
                var tileWidget = Object.Instantiate(_prefab, _container);
                tileWidget.gameObject.SetActive(false);
                _pool.Add(tileWidget);
            }
        }

        public void Clear() {
            foreach (var tileWidget in _pool) {
                Object.Destroy(tileWidget.gameObject);
            }

            _pool.Clear();
        }

        public void Dispose() {
            if (_isDisposed) {
                return;
            }

            try {
                Clear();
            }
            catch (Exception e) {
                Debug.LogException(e);
            }
        }
    }
}