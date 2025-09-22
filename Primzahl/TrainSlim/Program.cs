using System;
using System.Collections.Generic;
using System.Threading;

string rail = new string('=', 70);
List<int> trainpos = new List<int>();
SemaphoreSlim semaphore = new SemaphoreSlim(1, 1);
bool running = true;

Console.Clear();


string leitungen = "";
string masten = "";

for (int i = 0; i < rail.Length; i++)
{
    if (i % 10 == 0 && i != 0) 
    {
        leitungen += "\\";
        masten += "|";
    }
    else
    {
        leitungen += " ";
        masten += " ";
    }
}


Console.SetCursorPosition(0, 1);
Console.WriteLine(leitungen);
Console.SetCursorPosition(0, 2);
Console.WriteLine(masten);

Thread renderThread = new Thread(() =>
{
    while (running)
    {
        semaphore.Wait();


        char[] zeile = rail.ToCharArray();

        foreach (var pos in trainpos)
        {
            if (pos >= 0 && pos < rail.Length - 1)
            {
                zeile[pos] = '|';
                zeile[pos + 1] = '|';
            }
        }


        Console.SetCursorPosition(0, 3);
        Console.WriteLine(new string(zeile) + "   ");


        for (int i = 0; i < trainpos.Count; i++)
        {
            trainpos[i]++;
        }


        trainpos.RemoveAll(p => p >= rail.Length);

        semaphore.Release();
        Thread.Sleep(100);
    }
});
renderThread.Start();

ConsoleKey key;
do
{
    key = Console.ReadKey(true).Key;

    if (key == ConsoleKey.Spacebar)
    {
        semaphore.Wait();
        
        if (trainpos.Count == 0 || trainpos[^1] > 5)
        {
            trainpos.Add(0);
        }
        semaphore.Release();
    }

} while (key != ConsoleKey.Escape);

running = false;
renderThread.Join();
  