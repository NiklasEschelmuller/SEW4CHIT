namespace BlazorApp1;

public class TttGame :  AWinner,IGame
{
 private readonly string?[,] _field = new string[3, 3];

    public string? Winner { get; private set; }
    public string NextPlayer { get; private set; } = "O";

    public string? this[int row, int col] => _field[row, col] ?? " ";

    public void Set(int row, int col)
    {
        if (_field[row, col] == null && Winner == null)
        {
            _field[row, col] = NextPlayer;
            Winner = CheckWinner();
            if (Winner == "No Winner / Draw" && IsDraw()) 
            {
                Winner = "Draw";
            }
            else
            {
                if (NextPlayer == "X")
                {
                    NextPlayer = "O";
                }
                else
                {
                    NextPlayer = "X";
                }
            }
        }
    }

    private bool IsDraw()
    {
        if (Winner != null) return false;

        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                if (_field[i, j] == null) return false;
            }
        }
        return true;
    }

    protected override string? HorizontalWinner()
    {
        for (int i = 0; i < 3; i++)
        {
            if (_field[i, 0] != null && _field[i, 0] == _field[i, 1] && _field[i, 1] == _field[i, 2])
                return _field[i, 0];
        }
        return null;
    }

    protected override string? VerticalWinner()
    {
        for (int i = 0; i < 3; i++)
        {
            if (_field[0, i] != null && _field[0, i] == _field[1, i] && _field[1, i] == _field[2, i])
                return _field[0, i];
        }
        return null;
    }

    protected override string? DiagonalWinner1()
    {
        if (_field[0, 0] != null && _field[0, 0] == _field[1, 1] && _field[1, 1] == _field[2, 2])
            return _field[0, 0];
        return null;
    }

    protected override string? DiagonalWinner2()
    {
        if (_field[0, 2] != null && _field[0, 2] == _field[1, 1] && _field[1, 1] == _field[2, 0])
            return _field[0, 2];
        return null;
    }
    
}
