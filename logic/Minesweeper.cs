using System;
namespace BlazorApp1;

public class Minesweeper
{
    private int[,] _field_;
    private bool[,] opened;
    
    public Minesweeper(int columns, int rows)
    {
        _field_ = new int[columns, rows];
        opened = new bool[columns, rows];
    }

    /*private void MineSet()
    {
        int amountm = _rand.Next(5, 20);
        while (amountm > 0)
        {
            int x = _rand.Next(0, 16);
            int y = _rand.Next(0, 16);
            if (_field[x, y] == null)
            {
                _field[x, y] = "!!";
                amountm--;
            }
            

        }
    }*/



   public string this [int col, int row] 
   {
       get
       {
           return _field_[col, row].ToString();
       }
   }
}


