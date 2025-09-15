// See https://aka.ms/new-console-template for more information
using monitorexample;

object monitorObj = new object();
Ping prod = new Ping(monitorObj);
Pong cons = new Pong(monitorObj);

Thread thread1 = new Thread(new ThreadStart(prod.Player1));
Thread thread2 = new Thread(new ThreadStart(cons.Player2));

thread1.Start();
thread2.Start();