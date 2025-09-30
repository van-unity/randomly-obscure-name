using System;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UnityEngine;

namespace RON.Scripts {
    public class Board : MonoBehaviour {
        [SerializeField] private LevelConfiguration _levelConfiguration;

        private TileContainer _tileContainer;
        private InputManager _inputManager;
        private TilePool _tilePool;
        private int[,] _matrix;
        private TileWidget[,] _tiles;

        private List<BoardPosition> _selection;
        private HashSet<BoardPosition> _busy;

        public event Action<TurnCompleteEventArgs> TurnComplete;
        public event Action BoardCleared;
        public event Action TileClicked;

        private void Start() {
            _busy = new HashSet<BoardPosition>();
            var context = FindObjectOfType<GameplayContext>();
            if (context == null) {
                Debug.LogError("No gameplay context found!");
                return;
            }

            _inputManager = context.InputManager;
            _tilePool = context.TilePool;
            var tileBounds = context.TileWidgetBounds;
            _tileContainer =
                context.TileContainerFactory.Create(_levelConfiguration.Columns, _levelConfiguration.Rows, tileBounds);
            _matrix = LevelMatrixGenerator.GenerateIndices(_levelConfiguration);

            var rows = _matrix.GetLength(0);
            var columns = _matrix.GetLength(1);
            _tiles = new TileWidget[rows, columns];
            for (int r = 0; r < rows; r++) {
                for (int c = 0; c < columns; c++) {
                    var tileWidget = _tilePool.Get();
                    tileWidget.transform.SetParent(transform);
                    tileWidget.name = $"Tile [{r}, {c}]";
                    tileWidget.SetTileSprite(_levelConfiguration.Sprites[_matrix[r, c]]);
                    _tiles[r, c] = tileWidget.GetComponent<TileWidget>();
                    _tileContainer.AddTile(tileWidget.transform);
                    tileWidget.gameObject.SetActive(true);
                }
            }

            _selection = new List<BoardPosition>(_levelConfiguration.MatchCount);

            var revealSequence = DOTween.Sequence();
            var hideSequence = DOTween.Sequence();
            for (int r = 0; r < rows; r++) {
                for (int c = 0; c < columns; c++) {
                    var tile = _tiles[r, c];
                    revealSequence.Join(tile.Reveal());
                    hideSequence.Join(tile.Hide());
                }
            }

            DOTween.Sequence()
                .AppendInterval(0.5f)
                .Append(revealSequence)
                .AppendInterval(3)
                .Append(hideSequence);

            _inputManager.Clicked += OnClick;
        }

        private void OnClick(Vector3 clickPos) {
            if (!_tileContainer.TryGetBoardPosition(clickPos, out var boardPosition)) {
                return;
            }

            if (!TryMarkBusy(boardPosition)) {
                return;
            }
            
            TileClicked?.Invoke();

            _selection.Add(boardPosition);
            if (_selection.Count == _levelConfiguration.MatchCount) {
                var selectionCopy = new List<BoardPosition>(_selection);
                _selection.Clear();
                OnSelectionComplete(selectionCopy);
            } else {
                _tiles[boardPosition.row, boardPosition.column].Reveal();
            }
        }

        private bool TryMarkBusy(BoardPosition p) => _busy.Add(p);
        private void UnmarkBusy(BoardPosition p) => _busy.Remove(p);

        private bool IsSelectionCorrect(List<BoardPosition> selection) {
            return selection.All(s => _matrix[s.row, s.column] == _matrix[selection[0].row, selection[0].column]);
        }

        private void OnSelectionComplete(List<BoardPosition> selection) {
            var tiles = new List<TileWidget>(selection.Count);

            for (int i = 0; i < selection.Count; i++) {
                tiles.Add(_tiles[selection[i].row, selection[i].column]);
            }

            if (IsSelectionCorrect(selection)) {
                TurnComplete?.Invoke(new TurnCompleteEventArgs(true));

                foreach (var position in selection) {
                    _tiles[position.row, position.column] = null;
                    _matrix[position.row, position.column] = -1;
                }

                tiles.Last().Reveal().OnComplete(() => {
                    var matchSequence = DOTween.Sequence();
                    foreach (var tileWidget in tiles) {
                        matchSequence.Join(tileWidget.Match());
                    }

                    matchSequence.OnComplete(() => {
                        foreach (var tileWidget in tiles) {
                            _tilePool.Return(tileWidget);
                            if (IsBoardCleared()) {
                                BoardCleared?.Invoke();
                            }
                        }
                    });
                });
            } else {
                TurnComplete?.Invoke(new TurnCompleteEventArgs(false));

                tiles.Last()
                    .Reveal()
                    .OnComplete(() => {
                        foreach (var position in selection) {
                            var tile = _tiles[position.row, position.column];
                            tile.Hide().OnComplete(() => { UnmarkBusy(position); });
                        }
                    });
            }
        }

        private bool IsBoardCleared() {
            int rows = _matrix.GetLength(0);
            int cols = _matrix.GetLength(1);

            for (int r = 0; r < rows; r++) {
                for (int c = 0; c < cols; c++) {
                    if (_matrix[r, c] != -1)
                        return false;
                }
            }

            return true;
        }

        private void OnDestroy() {
            try {
                _inputManager.Clicked -= OnClick;
            }
            catch (Exception e) {
                Debug.LogException(e);
            }
        }
    }
}