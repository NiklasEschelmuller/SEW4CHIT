using System;
using System.Collections.Generic;
using System.Threading;

//Was noch zu erldigen ist.
//Zug soll wenn er bei sationen ist blockieren so dass andere nicht da sind später soll er unterschiedlich lang sein
//und die section sollen am anfang gespert sein so das man 1 drückt und der 1 wird frei etc. 
//erste punkt soll keine stange sein. Output gibt an wenn zug station schließt oder wenn zug stion verlässt also sie wieder realseased

string rail = new string('=', 50);

List<(int pos, int length)> trains = new List<(int pos, int length)>();
SemaphoreSlim semaphore = new SemaphoreSlim(1, 1);
bool running = true;

// Masten entlang der Strecke
int[] mastPositions = new int[10];
for (int i = 0; i < mastPositions.Length; i++)
    mastPositions[i] = i * 7 + 5;

SemaphoreSlim[] mastSemaphores = new SemaphoreSlim[mastPositions.Length];
for (int i = 0; i < mastSemaphores.Length; i++)
    mastSemaphores[i] = new SemaphoreSlim(1, 1); // frei

// Stationen (jede Station = 1 Mast, der blockiert)
int[] stationIndices = new int[5] { 0, 1, 2, 3, 4 }; // Mastindex der Station
bool[] stationUnlocked = new bool[stationIndices.Length]; // manuell freigeben

Console.Clear();

void DrawRail()
{
    char[] topLine = new char[rail.Length];
    char[] bottomLine = rail.ToCharArray();

    for (int i = 0; i < rail.Length; i++)
        topLine[i] = ' ';

    // Masten zeichnen
    for (int i = 0; i < mastPositions.Length; i++)
    {
        if (mastPositions[i] < rail.Length)
            topLine[mastPositions[i]] = '\\';
    }

    // Züge zeichnen
    foreach (var train in trains)
    {
        int pos = train.pos;
        int length = train.length;
        for (int j = 0; j < length && pos + j < rail.Length; j++)
            bottomLine[pos + j] = '|';
    }

    // Blockierte Stationen (nur die Mastspitze markieren)
    for (int s = 0; s < stationIndices.Length; s++)
    {
        int mast = mastPositions[stationIndices[s]];
        if (mastSemaphores[stationIndices[s]].CurrentCount == 0)
            topLine[mast] = '_'; // Blockierter Mast
    }

    Console.SetCursorPosition(0, 0);
    Console.WriteLine(new string(topLine));
    Console.SetCursorPosition(0, 1);
    Console.WriteLine(new string(bottomLine));
}

Thread renderThread = new Thread(() =>
{
    while (running)
    {
        semaphore.Wait();

        // Züge bewegen
        for (int i = 0; i < trains.Count; i++)
        {
            var (pos, length) = trains[i];

            // Prüfen, ob Zug vor einem blockierten Mast steht
            bool blocked = false;
            for (int s = 0; s < stationIndices.Length; s++)
            {
                int mast = mastPositions[stationIndices[s]];
                if (pos + length >= mast && pos <= mast && mastSemaphores[stationIndices[s]].CurrentCount == 0)
                    blocked = true;
            }

            if (!blocked)
            {
                // Zug bewegen
                pos++;

                // Prüfen, ob Zug Mast betritt -> sperren
                for (int s = 0; s < stationIndices.Length; s++)
                {
                    int mast = mastPositions[stationIndices[s]];
                    if (pos + length - 1 == mast && mastSemaphores[stationIndices[s]].CurrentCount == 1)
                    {
                        mastSemaphores[stationIndices[s]].Wait();
                        Console.SetCursorPosition(0, 3 + i);
                        Console.WriteLine($"Zug {i} betritt Station {s + 1}");
                    }

                    // Zug verlässt Mast -> freigeben
                    if (pos > mast && mastSemaphores[stationIndices[s]].CurrentCount == 0)
                    {
                        mastSemaphores[stationIndices[s]].Release();
                        Console.SetCursorPosition(0, 3 + i + stationIndices.Length);
                        Console.WriteLine($"Zug {i} verlässt Station {s + 1}");
                    }
                }
            }

            trains[i] = (pos, length);
        }

        trains.RemoveAll(t => t.pos >= rail.Length);

        DrawRail();

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

    // Stationen manuell freigeben: 1..5
    if (key >= ConsoleKey.D1 && key <= ConsoleKey.D5)
    {
        int idx = key - ConsoleKey.D1;
        if (!stationUnlocked[idx])
        {
            mastSemaphores[stationIndices[idx]].Release();
            stationUnlocked[idx] = true;
            Console.WriteLine($"Station {idx + 1} freigegeben");
        }
    }

    // Neuen Zug starten
    if (key == ConsoleKey.Spacebar)
    {
        semaphore.Wait();

        int length = rand.Next(2, 5);

        // Prüfen, ob erste Station freigegeben
        bool canStart = mastSemaphores[stationIndices[0]].CurrentCount == 1;

        if (canStart)
        {
            trains.Add((0, length));
            Console.WriteLine($"Neuer Zug startet mit Länge {length}");
        }
        else
            Console.WriteLine("Start blockiert, Station 1 muss freigegeben werden");

        semaphore.Release();
    }

} while (key != ConsoleKey.Escape);

running = false;
renderThread.Join();
