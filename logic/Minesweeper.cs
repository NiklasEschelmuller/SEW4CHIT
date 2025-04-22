namespace BlazorApp1
{
    public class Minesweeper
    {
        private int[,] _field_;
        private bool[,] opened;
        private int columns, rows;

        public Minesweeper(int columns, int rows)
        {
            this.columns = columns;
            this.rows = rows;
            _field_ = new int[columns, rows];
            opened = new bool[columns, rows];
            SetMine();
            CalculateMineCounts();
        }

        private void SetMine()
        {
            _field_[0, 1] = -1;
            _field_[2, 1] = -1;
        }

        private void CalculateMineCounts() {
            for (int x = 0; x < columns; x++)
            {
                for (int y = 0; y < rows; y++)
                {
                    if (_field_[x, y] != -1)
                    {
                        int mineCount = 0;

                        for (int dx = -1; dx <= 1; dx++)
                        {
                            for (int dy = -1; dy <= 1; dy++)
                            {
                                if (dx == 0 && dy == 0)
                                    continue;

                                int nx = x + dx;
                                int ny = y + dy;

                                if (IsInBounds(nx, ny) && _field_[nx, ny] == -1)
                                {
                                    mineCount++;
                                }
                            }
                        }

                        _field_[x, y] = mineCount;
                    }
                }
            }
        }

        private bool IsInBounds(int x, int y)
        {
            return x >= 0 && x < columns && y >= 0 && y < rows;
        }

        public string this[int col, int row]
        {
            get
            {
                if (_field_[col, row] == -1) return "!!";
                return _field_[col, row].ToString();      
            }
        }
    }
}