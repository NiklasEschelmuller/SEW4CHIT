using System;
using System.Collections.Generic;
using System.Threading;
//Zug soll wenn er bei sationen ist blockieren so dass andere nicht da sind später soll er unterschiedlich lang sein
//und die section sollen am anfang gespert sein so das man 1 drückt und der 1 wird frei
string rail = new string('=', 70);
List<int> trainpos = new List<int>();
SemaphoreSlim semaphore = new SemaphoreSlim(1, 1);
bool running = true;

int[] mastPositions = new int[10];
for (int i = 0; i < mastPositions.Length; i++)
    mastPositions[i] = i * 5;

SemaphoreSlim[] mastSemaphores = new SemaphoreSlim[mastPositions.Length];
for (int i = 0; i < mastSemaphores.Length; i++)
    mastSemaphores[i] = new SemaphoreSlim(1, 1);

Console.Clear();


void DrawMasts(char[] mastLine1, char[] mastLine2)
{
    Console.SetCursorPosition(0, 1);
    Console.WriteLine(new string(mastLine1));
    Console.SetCursorPosition(0, 2);
    Console.WriteLine(new string(mastLine2));
}

Thread renderThread = new Thread(() =>
{
    while (running)
    {
        semaphore.Wait();


        char[] mastLine1 = new char[rail.Length];
        char[] mastLine2 = new char[rail.Length];
        for (int i = 0; i < rail.Length; i++)
        {
            mastLine1[i] = ' ';
            mastLine2[i] = ' ';
        }
        
        for (int i = 0; i < mastPositions.Length; i++)
        {
            mastLine1[mastPositions[i]] = '\\';
            mastLine2[mastPositions[i]] = '|';
        }
        
        char[] zeile = rail.ToCharArray();
        foreach (var pos in trainpos)
        {
            if (pos >= 0 && pos < rail.Length - 1)
            {
                zeile[pos] = '|';
                zeile[pos + 1] = '|';
                
                for (int m = 0; m < mastPositions.Length; m++)
                {
                    if (pos == mastPositions[m])
                        mastLine1[mastPositions[m]] = '-';
                }
            }
        }

        DrawMasts(mastLine1, mastLine2);
        Console.SetCursorPosition(0, 3);
        Console.WriteLine(new string(zeile) + "   ");


        for (int i = 0; i < trainpos.Count; i++)
        {

            bool blocked = false;
            for (int m = 0; m < mastPositions.Length; m++)
            {
                if (trainpos[i] + 1 == mastPositions[m])
                {
                    mastSemaphores[m].Wait();
                    blocked = true;
                }
            }

            trainpos[i]++;
            
            if (blocked)
            {
                for (int m = 0; m < mastPositions.Length; m++)
                {
                    if (trainpos[i] > mastPositions[m] && mastSemaphores[m].CurrentCount == 0)
                        mastSemaphores[m].Release();
                }
            }
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

        bool canStart = true;
        for (int m = 0; m < mastPositions.Length; m++)
        {
            if (mastPositions[m] == 0 && mastSemaphores[m].CurrentCount == 0)
                canStart = false;
        }
        if (canStart)
            trainpos.Add(0);
        semaphore.Release();
    }

} while (key != ConsoleKey.Escape);

running = false;
renderThread.Join();
