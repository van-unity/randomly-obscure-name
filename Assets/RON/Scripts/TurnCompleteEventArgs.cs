using System;

namespace RON.Scripts {
    public class TurnCompleteEventArgs : EventArgs {
        public bool IsCorrect { get; }

        public TurnCompleteEventArgs(bool isCorrect) {
            IsCorrect = isCorrect;
        }
    }
}