using System.Runtime.CompilerServices;

namespace TrainSlimOb;

public class TrainObserver
{
    public void UpdateTrain(object sender, EventArgs e)
    {
        TrainSubject Train = (sender as TrainSubject);
        
        Globals.screen.WaitOne();
        
     //Draw Train
     Console.SetCursorPosition(Train.CurrPos, Globals.DisplayLine);

     if (Train.CurrPos < Console.WindowWidth - Train.Size) //inheralb der Simulation Area
     {
         Console.Write("|"); //vorne zeichnen
         Console.SetCursorPosition(Train.CurrPos - Train.Size, Globals.DisplayLine);
         Console.Write("="); //hinten löschen
     }
     else//outsie
     {
         Console.SetCursorPosition(Train.CurrPos - Train.Size, Globals.DisplayLine);
        for(int i = 0; i < Train.Size; i++){
            Console.Write("="); //Remove Train from Display
         }
     }
     
     Globals.screen.Release();
    }
}