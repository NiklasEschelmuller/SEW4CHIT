using System.Net;
using System.Net.Sockets;
using System.Text;
//Zum Aufrufen der Website
// http://localhost:2025/index.html
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

            // Restliche Headerzeilen überspringen
            while (!string.IsNullOrEmpty(sr.ReadLine())) ;

            var parts = request.Split(' ');
            if (parts.Length < 2) continue;

            string file = parts[1].Substring(1);
            if (string.IsNullOrEmpty(file)) file = "index.html";

            if (File.Exists(file))
            {
                // Factory erstellt passenden Handler
                FileHandler handler = FileHandlerFactory.Create(file);

                // Header senden
                string header = $"HTTP/1.1 200 OK\r\n" +
                                $"Content-Type: {handler.ContentType}\r\n" +
                                $"Content-Length: {handler.GetContentLength(file)}\r\n" +
                                "Connection: close\r\n\r\n";
                byte[] headerBytes = Encoding.UTF8.GetBytes(header);
                st.Write(headerBytes, 0, headerBytes.Length);

                // Dateiinhalt senden
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

//Text
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

// Binärdateien (Bilder, PDFs, CSS, JS, JSON)
class BinaryFileHandler(string contentType) : FileHandler
{
    public override string ContentType => contentType;
    public override long GetContentLength(string path) => new FileInfo(path).Length;
    public override void Send(Stream stream, string path)
    {
        byte[] bytes = File.ReadAllBytes(path);
        stream.Write(bytes, 0, bytes.Length);
    }
}

// Factory
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
            ".gif" => new BinaryFileHandler("image/gif"),
            ".webp" => new BinaryFileHandler("image/webp"),
            ".pdf" => new BinaryFileHandler("application/pdf"),
            ".css" => new BinaryFileHandler("text/css"),
            ".js" => new BinaryFileHandler("application/javascript"),
            ".json" => new BinaryFileHandler("application/json"),
            _ => new BinaryFileHandler("application/octet-stream")
        };
    }
}