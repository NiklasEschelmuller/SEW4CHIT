namespace BlazorApp1.logic;

public class Minesweeper: IGame
{
    private readonly string?[,] _field = new string[15, 15];

    public string? Winner { get; private set; }
    
    
    public string NextPlayer { get; private set; } = "O";
    public string? this[int x, int y] => _field[x, y] ?? " ";

    public void Set(int x, int y)
    {
        if (_field[x, y] == null && Winner == null)
        {
            _field[x, y] = NextPlayer;
            //Winner = CheckWinner();
            
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