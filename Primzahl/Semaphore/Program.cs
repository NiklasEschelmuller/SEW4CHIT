// See https://aka.ms/new-console-template for more information

using System.Net.NetworkInformation;

SemaphoreSlim ping = new SemaphoreSlim(1, 1);
//Ping Fahne ist oben
SemaphoreSlim pong = new SemaphoreSlim(0, 1);
//Pong Fahne ist unten
new Thread(new ThreadStart(() => Ping())).Start();
new Thread(new ThreadStart(() => Pong())).Start();

void Ping()
{
    for (int i = 0; i < 10; i++)
    {
        ping.Wait(); //eigne Fahne ("ping") runter
        Console.WriteLine("Ping");
        pong.Release(); //andere Fahne("pong") hoch
    }
}

void Pong()
{
    while (true)
    {
        pong.Wait(); //eigene Fahne ("pong") runter
        Console.WriteLine("Pong");
        ping.Release(); //andere Fahne("ping") hoch
    }
}