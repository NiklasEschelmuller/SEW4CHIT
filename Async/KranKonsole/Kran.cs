namespace KranKonsole;

public class Crane
{
    private readonly MaschineA _maschineA;
    private readonly MaschineB _maschineB;

    public Crane(MaschineA a, MaschineB b)
    {
        _maschineA = a;
        _maschineB = b;
    }

    public void Run()
    {
        int werkstueck = 1;

        while (true)
        {
            Console.WriteLine($"\n[Kran] Neues Werkstück {werkstueck} aus Lager 1 geholt.");
            Thread.Sleep(500);

            Console.WriteLine("[Kran] Transportiert zu Maschine A...");
            Thread.Sleep(500);
            _maschineA.GiveWork();
            _maschineA.WaitUntilDone();

            Console.WriteLine("[Kran] Transportiert zu Maschine B...");
            Thread.Sleep(500);
            _maschineB.GiveWork();
            _maschineB.WaitUntilDone();

            Console.WriteLine("[Kran] Transportiert ins Lager 2...");
            Thread.Sleep(500);

            Console.WriteLine($"[Kran] Werkstück {werkstueck} vollständig bearbeitet ✅");
            werkstueck++;
        }
    }
}