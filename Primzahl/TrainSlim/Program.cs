using System;
using System.Collections.Generic;
using System.Threading;
//Was noch zu erldigen ist.
//Zug soll wenn er bei sationen ist blockieren so dass andere nicht da sind später soll er unterschiedlich lang sein
//und die section sollen am anfang gespert sein so das man 1 drückt und der 1 wird frei etc. 
//erste punkt soll keine stange sein. Output gibt an wenn zug station schließt oder wenn zug stion verlässt also sie wieder realseased
string rail = new string('=', 70);

List<(int pos, int length)> trains = new List<(int pos, int length)>();

SemaphoreSlim semaphore = new SemaphoreSlim(1, 1);
bool running = true;


int[] mastPositions = new int[10];
for (int i = 0; i < mastPositions.Length; i++)
    mastPositions[i] = i * 10 + 10; 

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
            if (mastPositions[i] < rail.Length)
            {
                mastLine1[mastPositions[i]] = '\\';
                mastLine2[mastPositions[i]] = '|';
            }
        }
        
        char[] zeile = rail.ToCharArray();

        foreach (var train in trains)
        {
            int pos = train.pos;
            int length = train.length;

            for (int j = 0; j < length; j++)
            {
                if (pos + j >= 0 && pos + j < rail.Length)
                    zeile[pos + j] = '|';
            }


            foreach (int mast in mastPositions)
            {
                if (pos <= mast && mast < pos + length)
                    mastLine1[mast] = '-';
            }
        }


        DrawMasts(mastLine1, mastLine2);
        Console.SetCursorPosition(0, 3);
        Console.WriteLine(new string(zeile) + "   ");


        for (int i = 0; i < trains.Count; i++)
        {
            var (pos, length) = trains[i];

            bool blocked = false;
            for (int m = 0; m < mastPositions.Length; m++)
            {
                if (pos + length == mastPositions[m]) 
                {
                    mastSemaphores[m].Wait();
                    blocked = true;
                }
            }

            pos++;

            if (blocked)
            {
                for (int m = 0; m < mastPositions.Length; m++)
                {
                    if (pos > mastPositions[m] && mastSemaphores[m].CurrentCount == 0)
                        mastSemaphores[m].Release();
                }
            }

            trains[i] = (pos, length);
        }
        
        trains.RemoveAll(t => t.pos >= rail.Length);

        semaphore.Release();
        Thread.Sleep(100);
    }
});
renderThread.Start();

ConsoleKey key;
Random rand = new Random();

do
{
    key = Console.ReadKey(true).Key;

    if (key == ConsoleKey.Spacebar)
    {
        semaphore.Wait();


        int length = rand.Next(2, 5);

        bool canStart = true;
        for (int m = 0; m < mastPositions.Length; m++)
        {
            if (mastPositions[m] == 0 && mastSemaphores[m].CurrentCount == 0)
                canStart = false;
        }

        if (canStart)
            trains.Add((0, length));

        semaphore.Release();
    }

} while (key != ConsoleKey.Escape);

running = false;
renderThread.Join();