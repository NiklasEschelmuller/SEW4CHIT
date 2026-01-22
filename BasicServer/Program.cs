using System.Net;
using System.Net.Sockets;

TcpListener lsnr = new TcpListener(IPAddress.Loopback, 2025);
lsnr.Start();
const int ANZ = 5;

Console.WriteLine("Server started, listening on port 2025");
for (int i = 0; i < ANZ; i++)
{
    new Thread(() => SessionkeepingServer1()).Start();
}

void Server(){
    while (true)
    {
        try
        {
            Socket soc = lsnr.AcceptSocket();
            Console.WriteLine($"Connected to Remote Endpoint: {soc.RemoteEndPoint}");
            Stream st = new NetworkStream(soc);
            StreamReader sr = new StreamReader(st);
            StreamWriter sw = new StreamWriter(st) { AutoFlush = true };
            
            string request = sr.ReadLine();
            if (string.IsNullOrEmpty(request)) continue;

            Console.WriteLine($"Request: {request}");
            
            // Den Rest der Browser-Header auslesen, bis zur Leerzeile
            while (!string.IsNullOrEmpty(sr.ReadLine())) ;

            var parts = request.Split(' ');
            if (parts.Length < 2) continue;

            string file = parts[1].Substring(1);
            if (string.IsNullOrEmpty(file)) file = "index.html"; // Default-Datei

            if (File.Exists(file))
            {
                string content = File.ReadAllText(file);
                string response = "HTTP/1.1 200 OK\r\n" +
                                  "Content-Type: text/html; charset=utf-8\r\n" +
                                  $"Content-Length: {System.Text.Encoding.UTF8.GetByteCount(content)}\r\n" +
                                  "Connection: close\r\n" +
                                  "\r\n" + // Die wichtige Leerzeile zwischen Header und Body
                                  content;

                sw.Write(response);
            }
            else
            {
                string notFound = "HTTP/1.1 404 Not Found\r\nContent-Length: 0\r\nConnection: close\r\n\r\n";
                sw.Write(notFound);
            }
            
            soc.Close(); // Wichtig für Browser, damit sie wissen, dass die Übertragung fertig ist
        }  catch (Exception e)
        {
            
        }
    }
}

void SessionkeepingServer1()
{
    while (true)
    {
        Socket soc = lsnr.AcceptSocket();
        try
        {
            Console.WriteLine($"Connected to : {soc.RemoteEndPoint}");
            Stream st = new NetworkStream(soc);
            StreamReader sr = new StreamReader(st);
            StreamWriter sw = new StreamWriter(st);
            sw.AutoFlush = true;

            while (true)
            {
                string input = sr.ReadLine();
                Console.WriteLine($"Client Request Input: {input}");

                string content = File.ReadAllText(input);
                Console.WriteLine($"Send: {content}");
                sw.Write(content);
            }
        }
        catch (Exception e)
        {
            Console.WriteLine($"Broken: {soc.RemoteEndPoint}");
        }
        finally
        {
            soc.Close();
        }
    }
}


void SessionlessServer1()
{
    while (true)
    {
        Socket soc = lsnr.AcceptSocket();
        try
        {
            Console.WriteLine($"Connected to : {soc.RemoteEndPoint}");
            Stream st = new NetworkStream(soc);
            StreamReader sr = new StreamReader(st);
            StreamWriter sw = new StreamWriter(st);
            sw.AutoFlush = true;

            while (true)
            { 
            string input = sr.ReadLine();
            Console.WriteLine($"Client Request Input: {input}");

            string content = File.ReadAllText(input);
            Console.WriteLine($"Send: {content}");
            sw.Write(content);
        }

    }
        catch (Exception e)
        {
            Console.WriteLine($"Broken: {soc.RemoteEndPoint}");
        }
        finally
        {
            soc.Close();
        }
    }
}


void StatelessEchoServer() //Echo Client
{
    while (true)
    {
        Socket soc = lsnr.AcceptSocket();
        try
        {
            Console.WriteLine($"Connected to : {soc.RemoteEndPoint}");
            Stream st = new NetworkStream(soc);
            StreamReader sr = new StreamReader(st);
            StreamWriter sw = new StreamWriter(st);
            sw.AutoFlush = true;
            
            string input = sr.ReadLine();
            Console.WriteLine($"Client Request Input: {input}");
            sw.Write(input.ToUpper());
            sw.Flush();
    
        }
        catch (Exception e)
        {
            Console.WriteLine($"Broken: {soc.RemoteEndPoint}");
        }
        finally
        {
            soc.Close();
        }
    }
}

Console.ReadKey();

