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
        SetMine();
    }

    private void SetMine()
    {
        _field_[1, 1] = -1;
    }



   public string this [int col, int row] 
   {
       get
       {
           if (_field_[col, row] == -1) return "!!";
           return _field_[col, row].ToString();
       }
   }
}


