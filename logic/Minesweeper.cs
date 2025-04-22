namespace BlazorApp1
{
    public class Minesweeper
    {
        private int[,] _field_;
        private bool[,] opened;
        private int columns;
        private int rows;
        private readonly Random _rand = new();
        
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
            int amountm = _rand.Next(5, 10);
            while (amountm > 0)
            {
                int x = _rand.Next(0, columns); 
                int y = _rand.Next(0, rows); 

                if (_field_[x, y] == 0)
                {
                    _field_[x, y] = -1; 
                    amountm--;
                }

            }
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