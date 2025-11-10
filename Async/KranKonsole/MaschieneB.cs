namespace KranKonsole;

public class MaschineB
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
                    Monitor.Wait(_lock);
                }

                Console.WriteLine("[Maschine B] Bearbeitung gestartet...");
                Thread.Sleep(1000);
                Console.WriteLine("[Maschine B] Bearbeitung abgeschlossen.");

                _hasWork = false;
                Monitor.Pulse(_lock);
            }
        }
    }
    
    public void GiveWork()
    {
        lock (_lock)
        {
            _hasWork = true;
            Monitor.Pulse(_lock);
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
