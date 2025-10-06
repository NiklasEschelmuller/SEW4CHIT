
using TrainSlimOb;
string rail = new string('=', 80);

        int[] mastPositions = new int[10];
        for (int i = 0; i < mastPositions.Length; i++)
            mastPositions[i] = i * 7 + 5;

        int stationCount = 7;
        SemaphoreSlim[] stationLocks = new SemaphoreSlim[stationCount];
        bool[] stationUnlocked = new bool[stationCount];
        for (int i = 0; i < stationCount; i++)
            stationLocks[i] = new SemaphoreSlim(0, 1);

        var trainSubject = new TrainSubject();
        var trainObserver = new TrainObserver(rail);
        var mastObserver = new MastObserver(mastPositions, stationLocks);

        bool running = true;
        SemaphoreSlim semaphore = new SemaphoreSlim(1, 1);
        Random rand = new Random();

        Thread renderThread = new Thread(() =>
        {
            while (running)
            {
                semaphore.Wait();
                
                for (int i = 0; i < trainSubject.Trains.Count; i++)
                {
                    var (pos, length, section) = trainSubject.Trains[i];
                    int nextSection = section + 1;

                    bool blocked = nextSection < stationCount &&
                                   pos + length >= mastPositions[nextSection] &&
                                   stationLocks[nextSection].CurrentCount == 0;

                    if (!blocked)
                    {
                        pos++;

                        bool enterStation = false;
                        bool leaveStation = false;

                        if (nextSection < stationCount && pos + length - 1 >= mastPositions[nextSection])
                        {
                            enterStation = true;
                            if (section < stationCount) leaveStation = true;
                            section = nextSection;
                        }
                        else if (section < stationCount && pos > mastPositions[section])
                        {
                            leaveStation = true;
                            section++;
                        }

                        trainSubject.UpdateTrain(i, pos, section, enterStation, leaveStation);
                    }
                }

                trainSubject.Trains.RemoveAll(t => t.pos >= rail.Length);
                
                mastObserver.DrawMasts();
                trainObserver.DrawTrains(trainSubject.Trains);

                semaphore.Release();
                Thread.Sleep(100);
            }
        });
        renderThread.Start();

        ConsoleKey key;
        do
        {
            key = Console.ReadKey(true).Key;

            if (key >= ConsoleKey.D1 && key <= ConsoleKey.D0 + stationCount)
            {
                int idx = key - ConsoleKey.D1;
                if (!stationUnlocked[idx])
                {
                    stationUnlocked[idx] = true;
                    stationLocks[idx].Release();
                    Console.WriteLine($"Station {idx + 1} freigegeben");
                }
            }

            if (key == ConsoleKey.Spacebar)
            {
                semaphore.Wait();
                int length = rand.Next(2, 5);
                if (stationLocks[0].CurrentCount == 1)
                {
                    stationLocks[0].Wait();
                    trainSubject.AddTrain(length);
                    Console.WriteLine($"Neuer Zug startet mit Länge {length}");
                }
                else
                    Console.WriteLine("Start blockiert, Station 1 muss freigegeben werden");
                semaphore.Release();
            }

        } while (key != ConsoleKey.Escape);

        running = false;
        renderThread.Join();