namespace TrainSlimOb;

public class TrainSubject
{
    public int Size { get; set; }
    public int Nr { get; set; }
    public int CurrPos { get; set; } = 9;
    public int CurrSector { get; set; } = 1;

    public TrainSubject(int size, int nr)
    {
        Size = size;
        CurrPos = size;
        Nr = nr;
    }

    public event EventHandler OnTrainHasMoved;
    public event EventHandler<int> OnReleaseSector;
    public event EventHandler<int> OnEnteringSector;

    public void Move()
    {
        Console.WindowWidth = 89;
        while (CurrPos < Console.WindowWidth)
        {
            if (CurrPos % 10 == 0)
            {
                //alle 10 mal anschauen
                Globals.indermittn[CurrSector].WaitOne();
                if (OnTrainHasMoved != null)
                {
                    OnEnteringSector(this, CurrSector);
                }
                CurrSector++; //bin im nächsten Sektor
            } 
            System.Threading.Thread.Sleep(150);
            if(OnTrainHasMoved != null)
                OnTrainHasMoved(this, EventArgs.Empty);
            if ((CurrPos - Size + 1) % 10 == 0)//Ausfahrt aus Sektor
            {
                if (CurrSector > 2)
                {
                    //gibt voherigen Sektor wieder frei
                    if (OnReleaseSector != null) {
                        OnReleaseSector(this, CurrSector - 2);
                    }
                    System.Threading.Thread.Sleep(50);
                    //gibt vorgen Sektor wieder frei
                    Globals.indermittn[CurrSector - 2].Release();
                }
            }
            CurrPos++;
        }
        if (OnReleaseSector != null)
            OnReleaseSector(this, 0);
        Globals.indermittn[8].Release();
        
    }
    
}