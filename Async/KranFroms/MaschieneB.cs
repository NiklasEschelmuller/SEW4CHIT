namespace KranFroms;

public class MaschieneB
{
    public void Process(Form1 form)
    {
        form.semaphore.Wait();
        try
        {
            form.AnimateMaschineY(ref form.maschieneBY, form.kranY + 35, "[MaschieneB] hochfahren");
            form.SetWerkstueckColor(Color.Green);
            Thread.Sleep(500);
            form.AnimateMaschineY(ref form.maschieneBY, 200, "[MaschieneB] zurückfahren");
        }
        finally
        {
            form.semaphore.Release();
        }
    }
}
