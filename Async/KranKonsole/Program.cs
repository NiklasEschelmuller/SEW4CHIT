using KranKonsole;

        Console.WriteLine("Simulation gestartet");

        var maschineA = new MaschineA();
        var maschineB = new MaschineB();
        var crane = new Crane(maschineA, maschineB);
        
        Thread threadA = new Thread(maschineA.Run);
        Thread threadB = new Thread(maschineB.Run);
        Thread threadCrane = new Thread(crane.Run);

        threadA.Start();
        threadB.Start();
        threadCrane.Start();

        Console.ReadLine(); // hält  Programm offen
 






