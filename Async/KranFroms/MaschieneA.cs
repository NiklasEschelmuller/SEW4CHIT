namespace KranFroms;

public class MaschieneA
{
    public void Process(Form1 form)
    {
        // Maschine wartet auf Semaphore
        form.semaphore.Wait();
        try
        {
            form.AnimateMaschineY(ref form.maschieneAY, form.kranY + 35, "[MaschieneA] hochfahren");
            form.SetWerkstueckColor(Color.Blue);
            Thread.Sleep(500);
            form.AnimateMaschineY(ref form.maschieneAY, 200, "[MaschieneA] zurückfahren");
        }
        finally
        {
            form.semaphore.Release();
        }
    }
}