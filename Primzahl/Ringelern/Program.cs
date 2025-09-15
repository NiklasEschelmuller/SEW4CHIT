const int ANZ = 8;
const int START = 3;
SemaphoreSlim[] ringlern = new SemaphoreSlim[ANZ];


for (int i = 0; i < ANZ; i++)
{
    if (i == (START-1)) {
        ringlern[i] = new SemaphoreSlim(1, 1);
    }else {
        ringlern[i] = new SemaphoreSlim(0, 1);
    }
}

for (int i = 0; i < ANZ; i++)
{
    int id = i; 
    new Thread(() => Spieler(id)).Start();
}

void Spieler(int id)
{
    for (int i = 0; i < 10; i++)
    {
        ringlern[id].Wait();

        Console.WriteLine($"Player{id + 1}");

        int next = (id + 1) % ANZ;
        ringlern[next].Release();
    }
}