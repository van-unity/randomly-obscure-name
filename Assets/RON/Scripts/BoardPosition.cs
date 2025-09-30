using System;

namespace RON.Scripts {
    public readonly struct BoardPosition : IEquatable<BoardPosition> {
        public readonly int column;
        public readonly int row;

        public BoardPosition(int column, int row) {
            this.column = column;
            this.row = row;
        }

        public override string ToString() => $"C: {column}, R: {row}";

        public bool Equals(BoardPosition other) {
            return column == other.column && row == other.row;
        }

        public override bool Equals(object obj) {
            return obj is BoardPosition other && Equals(other);
        }

        public override int GetHashCode() {
            return HashCode.Combine(column, row);
        }
    }
}