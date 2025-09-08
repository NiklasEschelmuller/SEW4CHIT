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

int amount = 1_000_000; 
int count1 = 0, count2 = 0, count3= 0, count4 = 0, count5 = 0 ,count6 = 0, count7 = 0, count8 = 0;
long time1 = 0, time2 = 0, time3 = 0, time4 = 0, time5 = 0, time6 = 0, time7 = 0, time8 = 0;

//Mit 2 Threads 
/* //Thread 1 
Thread t1 = new Thread(() =>
{
    bool[] sieve = new bool[amount + 1];
    for (int i = 2; i <= amount; i++) sieve[i] = true;

    Stopwatch sw = Stopwatch.StartNew();
    for (int i = 2; i * i <= amount; i++)
    {
        if (sieve[i]) {
            for (int j = i * i; j <= amount; j += i)
                sieve[j] = false;
        }
    }
    sw.Stop();

    for (int i = 2; i<= amount /2;  i++)
        if (sieve[i]) count1++;

    time1 = sw.ElapsedMilliseconds;
});


//Thread 2
Thread t2 = new Thread(() =>
{
    bool[] sieve = new bool[amount + 1];
    for (int i = 2; i <= amount; i++) sieve[i] = true;

    Stopwatch sw = Stopwatch.StartNew();
    for (int i = 2; i * i <= amount; i++) {
        if (sieve[i]) {
            for (int j = i * i; j <= amount; j += i)
                sieve[j] = false;
        }
    }
    sw.Stop();

    for (int i = 2; i <= amount/2; i++)
        if (sieve[i]) count2++;

    time2 = sw.ElapsedMilliseconds;
});



Stopwatch total = Stopwatch.StartNew();
t1.Start();
t2.Start();


t1.Join();
t2.Join();
total.Stop();

//Ergebnis
int totalCount = count1 + count2;
Console.WriteLine($"Thread1: {count1} Primzahlen, Zeit: {time1} ms");
Console.WriteLine($"Thread2: {count2} Primzahlen, Zeit: {time2} ms");

Console.WriteLine($"\nGesamt: {totalCount} Primzahlen bis {amount}");
Console.WriteLine($"Gesamtzeit (parallel): {total.ElapsedMilliseconds} ms");
Console.WriteLine($"Gesamtzeit (addiert): {time1 + time2} ms");

*/

/*
 //mit 4 Threads
//Thread 1
Thread t1 = new Thread(() =>
{
    bool[] sieve = new bool[amount + 1];
    for (int i = 2; i <= amount; i++) sieve[i] = true;

    Stopwatch sw = Stopwatch.StartNew();
    for (int i = 2; i * i <= amount; i++)
    {
        if (sieve[i]) {
            for (int j = i * i; j <= amount; j += i)
                sieve[j] = false;
        }
    }
    sw.Stop();

    for (int i = 2; i<= amount /4;  i++)
        if (sieve[i]) count1++;

    time1 = sw.ElapsedMilliseconds;
});


//Thread 2
Thread t2 = new Thread(() =>
{
    bool[] sieve = new bool[amount + 1];
    for (int i = 2; i <= amount; i++) sieve[i] = true;

    Stopwatch sw = Stopwatch.StartNew();
    for (int i = 2; i * i <= amount; i++) {
        if (sieve[i]) {
            for (int j = i * i; j <= amount; j += i)
                sieve[j] = false;
        }
    }
    sw.Stop();

    for (int i = 2; i <= amount/4; i++)
        if (sieve[i]) count2++;

    time2 = sw.ElapsedMilliseconds;
});


//Thread 3
Thread t3 = new Thread(() =>
{
    bool[] sieve = new bool[amount + 1];
    for (int i = 2; i <= amount; i++) sieve[i] = true;

    Stopwatch sw = Stopwatch.StartNew();
    for (int i = 2; i * i <= amount; i++)
    { if (sieve[i]) {
            for (int j = i * i; j <= amount; j += i)
                sieve[j] = false;
        }
    }
    sw.Stop();

    for (int i = 2; i <= amount/4; i++)
        if (sieve[i]) count3++;

    time3 = sw.ElapsedMilliseconds;
});

//Thread4
Thread t4 = new Thread(() =>
{
    bool[] sieve = new bool[amount + 1];
    for (int i = 2; i <= amount; i++) sieve[i] = true;

    Stopwatch sw = Stopwatch.StartNew();
    for (int i = 2; i * i <= amount; i++)
    {
        if (sieve[i])
        {
            for (int j = i * i; j <= amount; j += i)
                sieve[j] = false;
        }
    }
    sw.Stop();

    for (int i = 2; i <= amount/4; i++)
        if (sieve[i]) count4++;

    time4 = sw.ElapsedMilliseconds;
});

Stopwatch total = Stopwatch.StartNew();
t1.Start();
t2.Start();
t3.Start();
t4.Start();

t1.Join();
t2.Join();
t3.Join();
t4.Join();
total.Stop();

//Ergebnis
int totalCount = count1 + count2 + count3 + count4;
Console.WriteLine($"Thread1: {count1} Primzahlen, Zeit: {time1} ms");
Console.WriteLine($"Thread2: {count2} Primzahlen, Zeit: {time2} ms");
Console.WriteLine($"Thread2: {count3} Primzahlen, Zeit: {time3} ms");
Console.WriteLine($"Thread2: {count4} Primzahlen, Zeit: {time4} ms");

Console.WriteLine($"\nGesamt: {totalCount} Primzahlen bis {amount}");
Console.WriteLine($"Gesamtzeit (parallel): {total.ElapsedMilliseconds} ms");
Console.WriteLine($"Gesamtzeit (addiert): {time1 + time2 + time3 + time4} ms");

*/

 //mit 8 Threads
 
 //Thread 1
Thread t1 = new Thread(() =>
{
    bool[] sieve = new bool[amount + 1];
    for (int i = 2; i <= amount; i++) sieve[i] = true;

    Stopwatch sw = Stopwatch.StartNew();
    for (int i = 2; i * i <= amount; i++)
    {
        if (sieve[i]) {
            for (int j = i * i; j <= amount; j += i)
                sieve[j] = false;
        }
    }
    sw.Stop();

    for (int i = 2; i<= amount /8;  i++)
        if (sieve[i]) count1++;

    time1 = sw.ElapsedMilliseconds;
});


//Thread 2
Thread t2 = new Thread(() =>
{
    bool[] sieve = new bool[amount + 1];
    for (int i = 2; i <= amount; i++) sieve[i] = true;

    Stopwatch sw = Stopwatch.StartNew();
    for (int i = 2; i * i <= amount; i++) {
        if (sieve[i]) {
            for (int j = i * i; j <= amount; j += i)
                sieve[j] = false;
        }
    }
    sw.Stop();

    for (int i = 2; i <= amount/8; i++)
        if (sieve[i]) count2++;

    time2 = sw.ElapsedMilliseconds;
});


//Thread 3
Thread t3 = new Thread(() =>
{
    bool[] sieve = new bool[amount + 1];
    for (int i = 2; i <= amount; i++) sieve[i] = true;

    Stopwatch sw = Stopwatch.StartNew();
    for (int i = 2; i * i <= amount; i++)
    { if (sieve[i]) {
            for (int j = i * i; j <= amount; j += i)
                sieve[j] = false;
        }
    }
    sw.Stop();

    for (int i = 2; i <= amount/8; i++)
        if (sieve[i]) count3++;

    time3 = sw.ElapsedMilliseconds;
});

//Thread4
Thread t4 = new Thread(() =>
{
    bool[] sieve = new bool[amount + 1];
    for (int i = 2; i <= amount; i++) sieve[i] = true;

    Stopwatch sw = Stopwatch.StartNew();
    for (int i = 2; i * i <= amount; i++)
    {
        if (sieve[i])
        {
            for (int j = i * i; j <= amount; j += i)
                sieve[j] = false;
        }
    }
    sw.Stop();

    for (int i = 2; i <= amount/8; i++)
        if (sieve[i]) count4++;

    time4 = sw.ElapsedMilliseconds;
});

//Thread 5
Thread t5 = new Thread(() =>
{
    bool[] sieve = new bool[amount + 1];
    for (int i = 2; i <= amount; i++) sieve[i] = true;

    Stopwatch sw = Stopwatch.StartNew();
    for (int i = 2; i * i <= amount; i++) {
        if (sieve[i]) {
            for (int j = i * i; j <= amount; j += i)
                sieve[j] = false;
        }
    }
    sw.Stop();

    for (int i = 2; i <= amount/8; i++)
        if (sieve[i]) count5++;

    time5 = sw.ElapsedMilliseconds;
});

//Thread 6
Thread t6 = new Thread(() =>
{
    bool[] sieve = new bool[amount + 1];
    for (int i = 2; i <= amount; i++) sieve[i] = true;

    Stopwatch sw = Stopwatch.StartNew();
    for (int i = 2; i * i <= amount; i++) {
        if (sieve[i]) {
            for (int j = i * i; j <= amount; j += i)
                sieve[j] = false;
        }
    }
    sw.Stop();

    for (int i = 2; i <= amount/8; i++)
        if (sieve[i]) count6++;

    time6 = sw.ElapsedMilliseconds;
});

//Thread 7
Thread t7 = new Thread(() =>
{
    bool[] sieve = new bool[amount + 1];
    for (int i = 2; i <= amount; i++) sieve[i] = true;

    Stopwatch sw = Stopwatch.StartNew();
    for (int i = 2; i * i <= amount; i++) {
        if (sieve[i]) {
            for (int j = i * i; j <= amount; j += i)
                sieve[j] = false;
        }
    }
    sw.Stop();

    for (int i = 2; i <= amount/8; i++)
        if (sieve[i]) count7++;

    time7 = sw.ElapsedMilliseconds;
});

//Thread 8
Thread t8 = new Thread(() =>
{
    bool[] sieve = new bool[amount + 1];
    for (int i = 2; i <= amount; i++) sieve[i] = true;

    Stopwatch sw = Stopwatch.StartNew();
    for (int i = 2; i * i <= amount; i++) {
        if (sieve[i]) {
            for (int j = i * i; j <= amount; j += i)
                sieve[j] = false;
        }
    }
    sw.Stop();

    for (int i = 2; i <= amount/8; i++)
        if (sieve[i]) count8++;

    time8 = sw.ElapsedMilliseconds;
});

Stopwatch total = Stopwatch.StartNew();
t1.Start();
t2.Start();
t3.Start();
t4.Start();
t5.Start();
t6.Start();
t7.Start();
t8.Start();

t1.Join();
t2.Join();
t3.Join();
t4.Join();
t5.Join();
t6.Join();
t7.Join();
t8.Join();
total.Stop();

//Ergebnis
int totalCount = count1 + count2 + count3 + count4+ count5 + count6 + count7 + count8;;
Console.WriteLine($"\nGesamt: {totalCount} Primzahlen bis {amount}");
Console.WriteLine($"Gesamtzeit (parallel): {total.ElapsedMilliseconds} ms");
Console.WriteLine($"Gesamtzeit (addiert): {time1 + time2 + time3 + time4 + time5 + time6 + time7 + time8} ms");
