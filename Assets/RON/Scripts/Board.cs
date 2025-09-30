using System;
using UnityEditor;
using UnityEngine;

namespace RON.Scripts {
    public class Board : MonoBehaviour {
        [SerializeField] private GameObject _tileWidgetPrefab;
        [SerializeField] private int _columns;
        [SerializeField] private int _rows;

        private TileContainer _tileContainer;

        private void Start() {
            var context = FindObjectOfType<GameplayContext>();
            if (context == null) {
                Debug.LogError("No gameplay context found!");
                return;
            }

            var tileBounds = _tileWidgetPrefab.transform.GetBounds();
            _tileContainer = context.TileContainerFactory.Create(_columns, _rows, tileBounds);
        }

        private void Update() {
            if (Input.GetKeyDown(KeyCode.Space)) {
                _tileContainer.AddTile(Instantiate(_tileWidgetPrefab).transform);
            }
        }

        private void OnDrawGizmos() {
            if (_tileContainer == null) {
                return;
            }

            var faceColor = Color.green;
            faceColor.a = .6f;
            Handles.DrawSolidRectangleWithOutline(_tileContainer.WorldRect, faceColor, Color.red);
        }
    }
}