namespace TrainSlimOb;
using System;
using System.Threading;

public class MastObserver
{
    
    public void EnterMast(object sender, int  sector)
    {
        TrainSubject Train = (sender as TrainSubject); 
        Globals.screen.WaitOne();
        
        Console.SetCursorPosition(0, 2+Train.Nr);
        Console.Write($"Train nr {Train.Nr}({Thread.CurrentThread.GetHashCode()}) enters sector {sector}");
        
        Console.SetCursorPosition(10*(sector), 0);
        Console.Write("-- ");
        
        Globals.screen.Release();
    }

    public void ReleaseMast( object sender, int sector)
    {
        TrainSubject Train = (sender as TrainSubject); 
        Globals.screen.WaitOne();
        
        Console.SetCursorPosition(0, 2+Train.Nr);
        Console.Write($"Train nr {Train.Nr}({Thread.CurrentThread.GetHashCode()}) releases section {sector}");
        Console.SetCursorPosition(10*(sector), 0);
        Console.Write("/  "); //Semaphore als frei zeichnen
        
        Globals.screen.Release();
    }
}