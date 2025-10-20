using TrainSlimOb;
using System;
using System.Collections.Generic;
using System.Threading;

Random randy = new Random();
Console.WindowWidth = Globals.ScreenWidth+10;
Console.CursorVisible = false;

//print rails
Console.SetCursorPosition(0, Globals.DisplayLine);
for (int i = 0; i < 90; i++)
{
    Console.Write("=");
}

//Zeichne UrsprungsGUI für Singalanlage
for (int i = 0; i < Globals.indermittn.Length; i++)
{
    Globals.indermittn[i] = new Semaphore(0, 1);
    if (i < Globals.indermittn.Length - 1)
    {
        Console.SetCursorPosition(10+10*(i%10), 1);
        Console.Write("|");
        Console.Write($"({i+1})");
        Console.SetCursorPosition(10+10*(i%10), 0);
        Console.Write("--");
    }
}

void MakeTrain()
{
    TrainObserver to = new TrainObserver();
    MastObserver fo = new MastObserver();
    int TrainCount = 0;
    TrainSubject ts = new TrainSubject(randy.Next(3, 7), TrainCount++);
    ts.OnTrainHasMoved += to.UpdateTrain;
    ts.OnReleaseSector += fo.ReleaseMast;
    ts.OnEnteringSector += fo.EnterMast;
    new Thread(() => ts.Move()).Start();
}

while (true)
{
    var rk = Console.ReadKey();
    if (rk.KeyChar == ' ')
    {
        MakeTrain();
    }
    else
    {
        try
        {
            Console.WriteLine('\b');
            Console.Write(' ');
            string k = rk.KeyChar.ToString();
            int nr = int.Parse(k);
            Globals.indermittn[nr].Release();
            Console.SetCursorPosition(10+10*((nr-10)%10), 0);
            Console.Write("/   ");
        }
        catch //Expection burying
        {
            
        }
    }
}

public static class Globals
{
    public static Semaphore screen = new Semaphore(1, 1);
    public static Semaphore[] indermittn = new Semaphore[9];
    public static int ScreenWidth = 80; //Anzeigelimiz
    public static int DisplayLine = 2;//Zeilen wo Zug gezeich net wird
    

}