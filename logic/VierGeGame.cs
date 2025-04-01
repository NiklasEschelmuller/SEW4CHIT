namespace BlazorApp1;

public class VierGeGame :  AWinner, IGameV
{
        private readonly int rows = 7;
        private readonly int cols = 7;
        private string[,] _field;
        private string _currentPlayer = "X";
        public string? Winner { get; private set; }

        public string[,] Field => _field;

        public VierGeGame()
        {
            _field = new string[rows, cols];
            for (int r = 0; r < rows; r++)
                for (int c = 0; c < cols; c++)
                    _field[r, c] = " ";
        }

        public void PlacePiece(int column)
        {
            if (Winner != null) return;

            for (int row = rows - 1; row >= 0; row--)
            {
                if (_field[row, column] == " ")
                {
                    _field[row, column] = _currentPlayer;
                    Winner = CheckWinner();
                    if (Winner == null)
                        _currentPlayer = (_currentPlayer == "X") ? "O" : "X";
                    return;
                }
            }
        }

        protected override string? HorizontalWinner()
        {
            for (int r = 0; r < rows; r++)
                for (int c = 0; c <= cols - 4; c++)
                    if (CheckLine(r, c, 0, 1)) return  "Gewinner: " +_field[r, c];

            return null;
        }

        protected override string? VerticalWinner()
        {
            for (int c = 0; c < cols; c++)
                for (int r = 0; r <= rows - 4; r++)
                    if (CheckLine(r, c, 1, 0)) return  "Gewinner: " +_field[r, c] ;

            return null;
        }

        protected override string? DiagonalWinner1()
        {
            for (int r = 0; r <= rows - 4; r++)
                for (int c = 0; c <= cols - 4; c++)
                    if (CheckLine(r, c, 1, 1)) return "Gewinner: " + _field[r, c] ;

            return null;
        }

        protected override string? DiagonalWinner2()
        {
            for (int r = 3; r < rows; r++)
                for (int c = 0; c <= cols - 4; c++)
                    if (CheckLine(r, c, -1, 1)) return "Gewinner: " + _field[r, c] ;

            return null;
        }

        private bool CheckLine(int row, int col, int dRow, int dCol)
        {
            string player = _field[row, col];
            if (player == " ") return false;

            for (int i = 1; i < 4; i++)
                if (_field[row + i * dRow, col + i * dCol] != player)
                    return false;

            return true;
        }
    }

