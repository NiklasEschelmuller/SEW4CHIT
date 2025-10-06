namespace TrainSlimOb;
using System.Threading;

public class MastObserver
{
    private readonly int[] mastPositions;
    private readonly SemaphoreSlim[] stationLocks;

    public MastObserver(int[] mastPositions, SemaphoreSlim[] stationLocks)
    {
        this.mastPositions = mastPositions;
        this.stationLocks = stationLocks;
    }

    public void DrawMasts()
    {
        char[] topLine = new char[mastPositions[^1] + 5];
        for (int i = 0; i < topLine.Length; i++) topLine[i] = ' ';

        for (int i = 0; i < mastPositions.Length; i++)
        {
            if (i < stationLocks.Length && stationLocks[i].CurrentCount == 0)
                topLine[mastPositions[i]] = '_';
            else
                topLine[mastPositions[i]] = '\\';
        }

        Console.SetCursorPosition(0, 0);
        Console.WriteLine(new string(topLine));
    }
}