using System;
using UnityEngine;

namespace RON.Scripts {
    public class ComboController : MonoBehaviour {
        [SerializeField] private ComboWidget _comboWidget;
        [SerializeField] private int _minCombo;

        private Board _board;
        private int _matchCount;

        private void Start() {
            var context = FindObjectOfType<GameplayContext>();
            if (!context) {
                Debug.LogError("No gameplay context found!");
                return;
            }
            _board = context.Board;
            
            _board.TurnComplete += BoardOnTurnComplete;
        }

        private void BoardOnTurnComplete(TurnCompleteEventArgs args) {
            if (args.IsCorrect) {
                if (++_matchCount >= _minCombo) {
                    _comboWidget.Show(_matchCount);
                }
            } else {
                _matchCount = 0;
            }
        }

        private void OnDestroy() {
            try {
                _board.TurnComplete -= BoardOnTurnComplete;
            }
            catch (Exception e) {
                Debug.LogException(e);
            }
        }
    }
}