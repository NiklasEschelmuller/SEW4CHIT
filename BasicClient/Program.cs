using System.Net.Sockets;

TcpClient client = new TcpClient("localhost", 2025);

Stream stream = client.GetStream();
stream.ReadTimeout = 1000;
Console.WriteLine($"Connected to server {client.Client.LocalEndPoint}");

StreamReader reader = new StreamReader(stream);
StreamWriter writer = new StreamWriter(stream);

writer.AutoFlush = true;


while (true)
{
    writer.WriteLine(Console.ReadLine());
    string response = reader.ReadLine();
    Console.WriteLine($"Response: {response}");

}

Console.ReadLine();
