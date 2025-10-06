using System;
using System.Collections.Generic;
using System.Threading;

//Aufgabe
//Aufteilen in Interrator Pattern?
//Zug soll wenn er bei sationen ist blockieren so dass andere nicht da sind später soll er unterschiedlich lang sein
//und die section sollen am anfang gespert sein so das man 1 drückt und der 1 wird frei etc. 
//erste punkt soll keine stange sein. Output gibt an wenn zug station schließt oder wenn zug stion verlässt also sie wieder realseased

string rail = new string('=', 50);

List<(int pos, int length, int section)> trains = new List<(int pos, int length, int section)>();
SemaphoreSlim semaphore = new SemaphoreSlim(1, 1);
bool running = true;

// Mastpositionen
int[] mastPositions = new int[10];
for (int i = 0; i < mastPositions.Length; i++)
    mastPositions[i] = i * 7 + 5;

//Staionen anzahl
int stationCount = 7;
if (stationCount > mastPositions.Length) stationCount = mastPositions.Length;

SemaphoreSlim[] stationLocks = new SemaphoreSlim[stationCount];
bool[] stationUnlocked = new bool[stationCount];
for (int i = 0; i < stationCount; i++)
    stationLocks[i] = new SemaphoreSlim(0, 1);

Console.Clear();

void SafeWriteLine(int left, int top, string text)
{
    if (top >= Console.BufferHeight)
        Console.WriteLine(text);
    else
    {
        Console.SetCursorPosition(left, top);
        Console.WriteLine(text);
    }
}

void DrawRail()
{
    char[] topLine = new char[rail.Length];
    char[] bottomLine = rail.ToCharArray();
    for (int i = 0; i < topLine.Length; i++) topLine[i] = ' ';

    // Masten zeichnen
    for (int i = 0; i < mastPositions.Length; i++)
        if (mastPositions[i] < rail.Length)
            topLine[mastPositions[i]] = '\\';
    
    for (int s = 0; s < stationCount; s++)
    {
        int mast = mastPositions[s];
        if (mast < rail.Length && stationLocks[s].CurrentCount == 0)
            topLine[mast] = '_';
    }

    // Züge zeichnen
    foreach (var train in trains)
    {
        for (int j = 0; j < train.length && train.pos + j < rail.Length; j++)
            bottomLine[train.pos + j] = '|';
    }

    SafeWriteLine(0, 0, new string(topLine));
    SafeWriteLine(0, 1, new string(bottomLine));
}

Thread renderThread = new Thread(() =>
{
    while (running)
    {
        semaphore.Wait();

        for (int i = 0; i < trains.Count; i++)
        {
            var (pos, length, section) = trains[i];

            int nextSection = section + 1;

            bool blockedByNext = false;
            if (nextSection < stationCount)
            {
                int nextMast = mastPositions[nextSection];
                if (pos + length >= nextMast && pos <= nextMast && stationLocks[nextSection].CurrentCount == 0)
                    blockedByNext = true;
            }

            if (!blockedByNext)
            {
                pos++;

                // Abschnittswechsel prüfen
                if (nextSection < stationCount)
                {
                    int nextMast = mastPositions[nextSection];

                    if (pos + length - 1 >= nextMast)
                    {
                        if (stationLocks[nextSection].CurrentCount == 1)
                        {
                            stationLocks[nextSection].Wait();
                            SafeWriteLine(0, 4 + i, $"Zug {i + 1} übernimmt Station {nextSection + 1}");
                        }

                        if (section >= 0 && section < stationCount)
                        {
                            if (stationLocks[section].CurrentCount == 0)
                            {
                                stationLocks[section].Release();
                                SafeWriteLine(0, 4 + i + stationCount, $"Zug {i + 1} gibt Station {section + 1} frei");
                            }
                        }

                        section = nextSection;
                    }
                }
                else
                {
                    if (section >= 0 && section < stationCount)
                    {
                        int mastHere = mastPositions[section];
                        if (pos > mastHere && stationLocks[section].CurrentCount == 0)
                        {
                            stationLocks[section].Release();
                            SafeWriteLine(0, 4 + i + stationCount, $"Zug {i + 1} gibt letzte Station {section + 1} frei");
                            section++;
                        }
                    }
                }
            }

            trains[i] = (pos, length, section);
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

    // Stationen manuell freigeben 
    if (key >= ConsoleKey.D1 && key <= ConsoleKey.D0 + stationCount)
    {
        int idx = key - ConsoleKey.D1;
        if (idx >= 0 && idx < stationCount && !stationUnlocked[idx])
        {
            stationUnlocked[idx] = true;
            stationLocks[idx].Release();
            Console.WriteLine($"Station {idx + 1} manuell freigegeben");
        }
    }

    // Neuen Zug starten
    if (key == ConsoleKey.Spacebar)
    {
        semaphore.Wait();

        int length = rand.Next(2, 5);

        if (stationCount > 0 && stationLocks[0].CurrentCount == 1)
        {
            stationLocks[0].Wait();
            trains.Add((0, length, 0));
            Console.WriteLine($"Neuer Zug startet mit Länge {length}");
        }
        else
        {
            Console.WriteLine("Start blockiert, Station 1 muss freigegeben werden");
        }

        semaphore.Release();
    }

} while (key != ConsoleKey.Escape);

running = false;
renderThread.Join();
