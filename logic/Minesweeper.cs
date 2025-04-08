using System;
namespace BlazorApp1;

public class Minesweeper
{
    private readonly string?[,] _field = new string[18, 18];
    private readonly Random _rand = new();

    public string? this[int x, int y] => _field[x, y] ?? " "; 
    public Minesweeper()
    {
        MineSet();

    }

    private void MineSet()
    {
        int amountm = _rand.Next(5, 20);
        while (amountm > 0)
        {
            int x = _rand.Next(0, 17);
            int y = _rand.Next(0, 17);
            if (_field[x, y] == null)
            {
                _field[x, y] = "!!";
                amountm--;
            }

        }
    }

    private void CheckforMines(int x, int y)
    {
        int counter = 0;
        for (int yi = y--; yi > y++; yi++)
        {
            for (int xi = x--; xi > x++; xi++)
            {
                if (_field[xi, yi] == "!!")
                {
                    counter++;
                }
            }
        }
         string c = counter.ToString();
        _field[x, y] = c;
        
         
    }

   public void Set(int x, int y)
    {

    }
}


