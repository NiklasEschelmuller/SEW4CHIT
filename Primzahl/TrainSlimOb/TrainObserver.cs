namespace TrainSlimOb;

public class TrainObserver
{
    private readonly string rail;

    public TrainObserver(string rail) => this.rail = rail;

    public void DrawTrains(System.Collections.Generic.List<(int pos, int length, int section)> trains)
    {
        char[] line = rail.ToCharArray();
        for (int i = 0; i < line.Length; i++) line[i] = '=';

        foreach (var train in trains)
        {
            for (int j = 0; j < train.length && train.pos + j < line.Length; j++)
                line[train.pos + j] = '|';
        }

        Console.SetCursorPosition(0, 1);
        Console.WriteLine(new string(line));
    }
}