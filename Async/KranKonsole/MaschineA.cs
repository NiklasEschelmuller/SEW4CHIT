namespace KranKonsole;

public class MaschineA
{
    private bool _hasWork = false;
    private readonly object _lock = new object();

    public void Run()
    {
        while (true)
        {
            lock (_lock)
            {
                if (!_hasWork)
                {
                    Monitor.Wait(_lock); // wartet auf Arbeit
                }

                Console.WriteLine("[Maschine A] Bearbeitung gestartet...");
                Thread.Sleep(1000);
                Console.WriteLine("[Maschine A] Bearbeitung abgeschlossen.");

                _hasWork = false;
                Monitor.Pulse(_lock); //  fertig
            }
        }
    }

    public void GiveWork()
    {
        lock (_lock)
        {
            _hasWork = true;
            Monitor.Pulse(_lock); // weckt  Maschine
        }
    }

    public void WaitUntilDone()
    {
        lock (_lock)
        {
            while (_hasWork)
            {
                Monitor.Wait(_lock);
            }
        }
    }
}
