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
        
        int x = _rand.Next(0, 17);
        int y = _rand.Next(0, 17);
        _field[x, y] = "!!";
    }

   public void Set(int x, int y)
    {
  
    }
}


