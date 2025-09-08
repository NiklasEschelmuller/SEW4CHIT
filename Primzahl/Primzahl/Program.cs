using System;
using System.Diagnostics;
using System.Threading;
/*
//Sieb des Eratosthenes
int amount = 1000000000;
bool[] primnumber = new bool[amount + 1];
for (int i = 2; i <= amount; i++) primnumber[i] = true;

Stopwatch sw = Stopwatch.StartNew();

for (int i = 2; i * i <= amount; i++)
{
    if (primnumber[i])
    {
        for (int j = i * i; j <= amount; j += i)
            primnumber[j] = false;
    }
}

int count = 0;
for (int i = 2; i <= amount; i++)
    if (primnumber[i]) count++;

sw.Stop();

Console.WriteLine($"Anzahl der Primzahlen bis {amount}: {count}");
Console.WriteLine($"Zeit: {sw.ElapsedMilliseconds} ms");


*/




/*
int amount = 10000000;
int count = 0;

Stopwatch sw = Stopwatch.StartNew();

for (int i = 2; i <= amount; i++)
{
    if (IsPrime(i))
        count++;
}

sw.Stop();

Console.WriteLine($"Anzahl der Primzahlen bis {amount}: {count}");
Console.WriteLine($"Zeit: {sw.ElapsedMilliseconds} ms");


static bool IsPrime(int n)
{
    if (n < 2) return false;
    if (n == 2) return true;
    if (n % 2 == 0) return false;

    int sqrt = (int)Math.Sqrt(n);
    for (int i = 3; i <= sqrt; i += 2)
    {
        if (n % i == 0)
            return false;
    }
    return true;
}
*/
//Aufgeteilt
 
using System;
using System.Diagnostics;
using System.Threading;

int amount = 1000000000; 
int count1 = 0, count2 = 0;
long time1 = 0, time2 = 0;

// Sekundär-Thread starten
Thread t = new Thread(() =>
{
    bool[] sieve2 = new bool[amount + 1];
    for (int i = 2; i <= amount; i++) sieve2[i] = true;

    Stopwatch sw2 = Stopwatch.StartNew();
    for (int i = 2; i * i <= amount; i++)
    {
        if (sieve2[i])
        {
            for (int j = i * i; j <= amount; j += i)
                sieve2[j] = false;
        }
    }
    sw2.Stop();

    for (int i = amount / 2 + 1; i <= amount; i++)
        if (sieve2[i]) count2++;

    time2 = sw2.ElapsedMilliseconds;
});

t.Start();

// Primär-Thread
bool[] sieve1 = new bool[amount + 1];
for (int i = 2; i <= amount; i++) sieve1[i] = true;

Stopwatch sw1 = Stopwatch.StartNew();
for (int i = 2; i * i <= amount; i++)
{
    if (sieve1[i])
    {
        for (int j = i * i; j <= amount; j += i)
            sieve1[j] = false;
    }
}
sw1.Stop();

for (int i = 2; i <= amount / 2; i++)
    if (sieve1[i]) count1++;

time1 = sw1.ElapsedMilliseconds;

// Warten bis Sekundär-Thread fertig
t.Join();

// Ergebnis
int total = count1 + count2;
Console.WriteLine($"Primär-Thread: {count1} Primzahlen, Zeit: {time1} ms");
Console.WriteLine($"Sekundär-Thread: {count2} Primzahlen, Zeit: {time2} ms");
Console.WriteLine($"\nGesamt: {total} Primzahlen bis {amount}");
Console.WriteLine($"Gesamtzeit (addiert): {time1 + time2} ms");