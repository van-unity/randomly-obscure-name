namespace RON.Scripts {
    public readonly struct BoardPosition {
        public readonly int column;
        public readonly int row;

        public BoardPosition(int column, int row) {
            this.column = column;
            this.row = row;
        }
    }
}