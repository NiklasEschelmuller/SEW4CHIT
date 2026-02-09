using System.Net;
using System.Net.Sockets;
using System.Text;

// --- Server Setup ---
TcpListener lsnr = new TcpListener(IPAddress.Loopback, 2025);
lsnr.Start();
const int anz = 5;

Console.WriteLine("Server started, listening on port 2025");
for (int i = 0; i < anz; i++)
{
    new Thread(Server).Start();
}


void Server()
{
    while (true)
    {
        try
        {
            Socket soc = lsnr.AcceptSocket();
            Console.WriteLine($"Connected to Remote Endpoint: {soc.RemoteEndPoint}");
            using Stream st = new NetworkStream(soc);
            using StreamReader sr = new StreamReader(st);
            using StreamWriter sw = new StreamWriter(st) { AutoFlush = true };

            string? request = sr.ReadLine();
            if (string.IsNullOrEmpty(request)) continue;
            Console.WriteLine($"Request: {request}");
            
            while (!string.IsNullOrEmpty(sr.ReadLine())) ;

            var parts = request.Split(' ');
            if (parts.Length < 2) continue;

            string file = parts[1].Substring(1);
            if (string.IsNullOrEmpty(file)) file = "index.html";

            if (File.Exists(file))
            {

                FileHandler handler = FileHandlerFactory.Create(file);
                string header = $"HTTP/1.1 200 OK\r\n" +
                                $"Content-Type: {handler.ContentType}\r\n" +
                                $"Content-Length: {handler.GetContentLength(file)}\r\n" +
                                "Connection: close\r\n\r\n";
                byte[] headerBytes = Encoding.UTF8.GetBytes(header);
                st.Write(headerBytes, 0, headerBytes.Length);
                
                handler.Send(st, file);
            }
            else
            {
                string notFound = "HTTP/1.1 404 Not Found\r\nContent-Length: 0\r\nConnection: close\r\n\r\n";
                byte[] notFoundBytes = Encoding.UTF8.GetBytes(notFound);
                st.Write(notFoundBytes, 0, notFoundBytes.Length);
            }

            soc.Close();
        }
        catch (Exception e)
        {
            Console.WriteLine($"Error: {e.Message}");
        }
    }
}


abstract class FileHandler
{
    public abstract string ContentType { get; }
    public abstract long GetContentLength(string path);
    public abstract void Send(Stream stream, string path);
}

// Text
class TextFileHandler : FileHandler
{
    public override string ContentType => "text/plain; charset=utf-8";
    public override long GetContentLength(string path) => Encoding.UTF8.GetByteCount(File.ReadAllText(path));
    public override void Send(Stream stream, string path)
    {
        byte[] bytes = Encoding.UTF8.GetBytes(File.ReadAllText(path));
        stream.Write(bytes, 0, bytes.Length);
    }
}

// HTML-Dateien
class HtmlFileHandler : FileHandler
{
    public override string ContentType => "text/html; charset=utf-8";
    public override long GetContentLength(string path) => Encoding.UTF8.GetByteCount(File.ReadAllText(path));
    public override void Send(Stream stream, string path)
    {
        byte[] bytes = Encoding.UTF8.GetBytes(File.ReadAllText(path));
        stream.Write(bytes, 0, bytes.Length);
    }
}

// PDF
class BinaryFileHandler : FileHandler
{
    private readonly string _contentType;
    public BinaryFileHandler(string contentType) => _contentType = contentType;
    public override string ContentType => _contentType;
    public override long GetContentLength(string path) => new FileInfo(path).Length;
    public override void Send(Stream stream, string path)
    {
        byte[] bytes = File.ReadAllBytes(path);
        stream.Write(bytes, 0, bytes.Length);
    }
}


static class FileHandlerFactory
{
    public static FileHandler Create(string filePath)
    {
        string ext = Path.GetExtension(filePath).ToLower();
        return ext switch
        {
            ".txt" => new TextFileHandler(),
            ".html" or ".htm" => new HtmlFileHandler(),
            ".png" => new BinaryFileHandler("image/png"),
            ".jpg" or ".jpeg" => new BinaryFileHandler("image/jpeg"),
            ".pdf" => new BinaryFileHandler("application/pdf"),
            ".json" => new BinaryFileHandler("application/json"),
            _ => new BinaryFileHandler("application/octet-stream")
        };
    }
}

/*
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
                string? input = sr.ReadLine();
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

*/