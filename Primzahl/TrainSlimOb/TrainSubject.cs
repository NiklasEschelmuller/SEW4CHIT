namespace TrainSlimOb;

public class TrainSubject
{
    public class TrainUpdate
    {
        public int TrainIndex;
        public int Pos;
        public int Section;
        public bool EnterStation;
        public bool LeaveStation;
    }

    public event Action<TrainUpdate> TrainUpdated;
    public List<(int pos, int length, int section)> Trains { get; private set; } = new();

    public void AddTrain(int length)
    {
        Trains.Add((0, length, 0));
        TrainUpdated?.Invoke(new TrainUpdate { TrainIndex = Trains.Count - 1, Pos = 0, Section = 0 });
    }

    public void UpdateTrain(int index, int pos, int section, bool enterStation = false, bool leaveStation = false)
    {
        var old = Trains[index];
        Trains[index] = (pos, old.length, section);
        TrainUpdated?.Invoke(new TrainUpdate
        {
            TrainIndex = index,
            Pos = pos,
            Section = section,
            EnterStation = enterStation,
            LeaveStation = leaveStation
        });
    }

    public void RemoveTrain(int index)
    {
        if (index >= 0 && index < Trains.Count) Trains.RemoveAt(index);
    }
}