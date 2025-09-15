namespace monitorexample;

class Ping {
    private readonly object obj;
    public Ping(object obj) {
        this.obj = obj;
    }
    public void Player1() {
        Monitor.Enter(obj);
        for (int i = 0; i <= 10; i++) {
            Var.thread1Waiting = true;
            if(Var.thread2Waiting == false) 
                Monitor.Wait(obj);

            Console.WriteLine("ping");

            Monitor.Pulse(obj);
            Var.thread2Waiting = false;
        }
        Var.finished = true;
        Monitor.Exit(obj);
    }
} 
