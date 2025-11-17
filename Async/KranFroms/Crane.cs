using KranFroms;

   public class Crane
    {
        private readonly MaschieneA maschieneA;
        private readonly MaschieneB maschieneB;
        private readonly Form1 form;

        public Crane(MaschieneA a, MaschieneB b, Form1 form)
        {
            maschieneA = a;
            maschieneB = b;
            this.form = form;
        }

        public void Run()
        {
            int werkstueckNumber = 1;

                form.semaphore.Wait(); // Kran blockiert Zugriff
                try
                {
                    // Lager 1 -> MaschieneA
                    form.AnimateKranX(50, "[Kran] zu Lager 1");
                    form.AnimateKranY(50, "[Kran] hebt Werkstück hoch");
                    form.AnimateKranX(250, "[Kran] Transport zu MaschieneA");

                    // Maschine A bearbeitet
                    form.semaphore.Release();
                    maschieneA.Process(form);
                    form.semaphore.Wait(); 

                    form.AnimateKranY(80, "[Kran] senkt Werkstück ab");

                    // MaschieneB
                    form.AnimateKranY(50, "[Kran] hebt Werkstück hoch");
                    form.AnimateKranX(450, "[Kran] Transport zu MaschieneB");

                    form.semaphore.Release();
                    maschieneB.Process(form);
                    form.semaphore.Wait();

                    form.AnimateKranY(80, "[Kran] senkt Werkstück ab");

                    // Lager 2
                    form.AnimateKranY(50, "[Kran] hebt Werkstück hoch");
                    form.AnimateKranX(650, "[Kran] zu Lager 2");
                    form.AnimateKranY(80, "[Kran] Werkstück ablegen");

                    form.SetStepText($"[Kran] Werkstück {werkstueckNumber} bearbeitet");
                    werkstueckNumber++;
                    Thread.Sleep(500);

                    // Reset
                    form.kranX = 50;
                    form.kranY = 80;
                    form.werkstueckX = 50;
                    form.werkstueckY = 150;
                    form.werkstueckColor = Color.Red;
                }
                finally
                {
                    form.semaphore.Release();
                }
            
        }
    }