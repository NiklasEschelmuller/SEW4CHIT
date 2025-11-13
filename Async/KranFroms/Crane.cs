using KranFroms;

public class Crane
{
    private readonly MaschieneA maschineA;
    private readonly MaschieneB maschineB;
    private readonly Form1 form;

    public Crane(MaschieneA a, MaschieneB b, Form1 form)
    {
        maschineA = a;
        maschineB = b;
        this.form = form;
    }

    public void Run()
    {
        int werkstueckNumber = 1;
        while (true)
        {
            // **Immer wieder von Lager 1 starten**
            form.werkstueckX = 50; 
            form.werkstueckY = 150;
            form.kranX = 50;

            // 1) Lager 1 -> Maschine A
            form.SetStepText($"[Kran] Werkstück {werkstueckNumber} aus Lager 1 geholt");
            form.AnimateWerkstueck(50, 250, $"[Kran] Transport zu Maschine A...");

            // Maschine A bearbeiten
            form.SetStepText("[Maschine A] Bearbeitung gestartet...");
            maschineA.Process();
            form.SetStepText("[Maschine A] Bearbeitung abgeschlossen");

            // 2) Maschine A -> Maschine B
            form.AnimateWerkstueck(250, 450, $"[Kran] Transport zu Maschine B...");

            // Maschine B bearbeiten
            form.SetStepText("[Maschine B] Bearbeitung gestartet...");
            maschineB.Process();
            form.SetStepText("[Maschine B] Bearbeitung abgeschlossen");

            // 3) Maschine B -> Lager 2
            form.AnimateWerkstueck(450, 650, $"[Kran] Transport ins Lager 2...");

            form.SetStepText($"[Kran] Werkstück {werkstueckNumber} vollständig bearbeitet ✅");

            Thread.Sleep(500);
            werkstueckNumber++;
        }
    }
}