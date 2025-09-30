using System;
using TMPro;
using UnityEngine;

namespace RON.Scripts {
    public class GameplayUi : MonoBehaviour {
        [SerializeField] private TextMeshProUGUI _matchesText;
        [SerializeField] private TextMeshProUGUI _turnsText;

        private Board _board;
        private int _matches;
        private int _turns;

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
                _matches += 1;
                _matchesText.text = _matches.ToString();
            }

            _turns += 1;
            _turnsText.text = _turns.ToString();
        }

        private void OnDestroy() {
            try {
                _board.TurnComplete -= BoardOnTurnComplete;
            }
            catch (Exception e) {
                Debug.LogError(e);
            }
        }
    }
}