using System;
using UnityEngine;

namespace RON.Scripts {
    public class SoundPlayer : MonoBehaviour {
        [SerializeField] private AudioSource _backgroundAudioSource;
        [SerializeField] private AudioSource _soundAudioSource;
        [SerializeField] private SoundsConfiguration _config;

        private Board _board;

        private void Start() {
            var context = FindObjectOfType<GameplayContext>();

            if (!context) {
                Debug.LogError("No gameplay context found!");
                return;
            }

            _board = context.Board;

            _board.TileClicked += BoardOnTileClicked;
            _board.TurnComplete += BoardOnTurnComplete;
        }

        private void BoardOnTurnComplete(TurnCompleteEventArgs args) {
            if (args.IsCorrect) {
                _soundAudioSource.PlayOneShot(_config.Match);
            } else {
                _soundAudioSource.PlayOneShot(_config.NoMatch);
            }
        }

        private void BoardOnTileClicked() {
            _soundAudioSource.PlayOneShot(_config.TileClicked);
        }

        private void OnDestroy() {
            try {
                _board.TileClicked -= BoardOnTileClicked;
                _board.TurnComplete -= BoardOnTurnComplete;
            }
            catch (Exception e) {
                Debug.LogException(e);
            }
        }
    }
}