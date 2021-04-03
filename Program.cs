using System;
using System.Linq;

namespace sudoku {
    class Program {
        static void Main(string[] args)
        {
            Console.WriteLine("Running sudoku, enjoy.");
            var b = new SudokuBoard(new int[]{
                5, 3, 4, 6, 7, 8, 9, 1, 2,
                6, 7, 2, 0, 9, 5, 3, 4, 8,
                1, 9, 8, 3, 4, 2, 5, 6, 7,
                8, 5, 9, 7, 6, 1, 4, 2, 3,
                4, 2, 6, 8, 5, 3, 7, 9, 1,
                7, 1, 3, 9, 2, 4, 8, 5, 6,
                9, 6, 1, 5, 3, 7, 2, 8, 4,
                2, 8, 7, 4, 1, 9, 6, 3, 5,
                3, 4, 5, 2, 8, 6, 1, 7, 9}
            );
            Console.WriteLine($"solvable: {b.Solve()}");
            Console.WriteLine(b);
        }
    }

    class SudokuBoard {
        // 1d array so we can utilize Array methods.
        public int[] board = new int[Width * Height];
        public const int Width = 9;
        public const int Height = 9;

        public SudokuBoard() =>
            Array.Fill(board, 0);

        public SudokuBoard(int[] b) =>
            Array.Copy(b, this.board, this.board.Length);

        public SudokuBoard(SudokuBoard b) =>
            Array.Copy(b.board, this.board, this.board.Length);

        public bool Solve(int row = 0, int col = 0)
        {
            for (row = 0; row < Height; row++) {
                for (col = 0; col < Width; col++) {
                    if (this[row, col] == 0) {
                        foreach (var i in Enumerable.Range(1, 9)) {
                            this[row, col] = i; // Try a number.
                            if (!ValidLocation(row, col, i)) { continue; }
                            if (Solve(row, col)) { return true; } // That number worked.
                        }
                        // No suitable number found, pop up one stack frame.
                        this[row, col] = 0;
                        return false;
                    }
                }
            }

            // If we reach the end, the board has to be solved.
            // NOTE: there may be *MULTIPLE SOLUTIONS*!
            return true;
        }

        private bool ValidLocation(int row, int col, int num) =>
            SquareIsUnique(row, col, num) &&
            RowIsUnique(row, col, num) &&
            CollumnIsUnique(row, col, num);

        bool RowIsUnique(int row, int col, int num) =>
            board.Skip(row * 9)
                .Take(9)
                .Where(x => x == this[row, col])
                .Count() == 1;

        bool CollumnIsUnique(int row, int col, int num) =>
            board.Where((_, i) => col % 9 == i % 9)
                .Where(x => x == this[row, col])
                .Count() == 1;

        bool WithinSquare(int row, int col, int idx)
        {
            var topLeftRow = (row / 3) * 3;
            var topLeftCol = (col / 3) * 3;
            var h = idx / 9;
            var w = idx % 9;
            return topLeftRow <= h
                && h <= topLeftRow + 2
                && topLeftCol <= w
                && w <= topLeftCol;
        }

        bool SquareIsUnique(int row, int col, int num) =>
            board.Where((_, i) => WithinSquare(row, col, i))
                .Where(x => x == num)
                .Count()
                .Equals(1);



        // Pretend a SudokuBoard is a 2d array.
        public int this[int row, int col] {
            get => this.board[(row * 9) + col];
            set => this.board[(row * 9) + col] = value;
        }

        // Ugly, don't read.
        public override string ToString()
        {
            var b = new System.Text.StringBuilder();
            int i = 0;
            foreach (int e in this.board) {
                i++;
                if ((i - 1) % 3 == 0) {
                    b.Append(" ");
                }
                b.Append(e);
                if (i % 9 == 0) {
                    b.Append("\n");
                } else {
                    b.Append(" ");
                }
                if (i % (9 * 3) == 0 && i < Width * Height - 1) {
                    b.Append("\n");
                }
            }
            return b.ToString();
        }
    }
}
