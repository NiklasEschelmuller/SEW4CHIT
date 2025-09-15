namespace monitorexample;

class Pong {
private readonly object obj;
public Pong(object obj) {
    this.obj = obj;
}
public void Player2() {
    Monitor.Enter(obj);
    if(Var.thread1Waiting)
        Monitor.Pulse(obj);

    Var.thread2Waiting = true; 
    while(Monitor.Wait(obj)) { 
        Console.WriteLine("pong");
        Monitor.Pulse(obj);

    }
    Monitor.Exit(obj);
}
} 
