namespace BlazorApp1
{
    //test
    public class Minesweeper
    {
        private int[,] _field_;
        private bool[,] opened;
        private bool[,] _suspected_mine;
        private const int amountmines = 10;
        Random rand = new Random();
        public int Columns => _field_.GetLength(0);
        public int Rows => _field_.GetLength(1);
        
        public int  SuspectCount { get; private set; }

        public Minesweeper(int columns, int rows)
        {
            _field_ = new int[columns, rows];
            opened = new bool[columns, rows];
            _suspected_mine = new bool[columns, rows];
            for (int i = 0; i < amountmines; i++)
            {
                int r = rand.Next(0, rows);
                int c = rand.Next(0, columns);
                if (_field_[c, r] != -1) // nicht zwei Minen an derselben Stelle
                {
                    _field_[c, r] = -1; // Mine
                    incrementBombCountsNearby(c, r);
                }
                else
                {
                    i--;
                }
            }
        }

        public void Toggle(int c, int r)
        {
            _suspected_mine[c, r] = !_suspected_mine[c, r];
            if (_suspected_mine[c, r])
            {
                SuspectCount++;
            }
            else
            {
                SuspectCount--;
            }
        }

       void incrementBombCountsNearby(int c, int r)
        {
            
            int[][] directions = [
                [-1, -1], [0, -1], [1, -1],
                [-1, 0], [1, 0],
                [-1, 1], [0, 1], [1, 1]
            ];
            for (int i = 0; i < directions.Length; i++)
            {
                int col = c + directions[i][0];
                int row = r + directions[i][1];
                if (col >= 0 && col < Columns && row >= 0 && row < Rows)
                {
                    incrementBombCount(col, row);
                }
            }
        }

        void incrementBombCount(int c, int r)
        {
            if (_field_[c, r] != -1)
                _field_[c, r]++;
        }



        public string this[int col, int row]
        {
            get
            {
                

                if (!opened[col, row] && !_suspected_mine[col, row])
                {
                    return String.Empty;
                }

                if (_suspected_mine[col, row])
                {
                    return "%";
                }

                if (_field_[col, row] == -1) return "!!";
                {
                    return _field_[col, row].ToString();
                }
            }
            set
            {
                if (!opened[col, row])
                {
                    Uncover(col, row);
                }
            }
        }

        private void Uncover(int c, int r)
        {
            if (opened[c, r])
            {
                return;
            }
            opened[c, r] = true;

            if (_field_[c, r] == 0)
            {
                int[][] directions = [
                    [-1, -1], [0, -1], [1, -1],
                    [-1, 0], [1, 0],
                    [-1, 1], [0, 1], [1, 1]
                ];
                for (int i = 0; i < directions.Length; i++)
                {
                    int col = c + directions[i][0];
                    int row = r + directions[i][1];
                    if (col >= 0 && col < Columns && row >= 0 && row < Rows)
                    {
                        Uncover(col, row);
                    }
                }
            }
        }
        
    }
}