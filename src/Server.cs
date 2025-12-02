using System.Net;
using System.Net.Sockets;
using System.Text;
using codecrafters_http_server;

// You can use print statements as follows for debugging, they'll be visible when running tests.
Console.WriteLine("Logs from your program will appear here!");

// Parse command-line arguments
string? filesDirectory = null;
for (int i = 0; i < args.Length; i++)
{
    if (args[i] == "--directory" && i + 1 < args.Length)
    {
        filesDirectory = args[i + 1];
        break;
    }
}

var router = new Router();
router.AddRoute("/", req => new HttpResponse { Body = "<h1>Hello</h1>" });
router.AddRoute("/echo/", req => new HttpResponse { ContentType = "text/plain", Body = req.Path.Split('/')[2] });
router.AddRoute("/user-agent", req => new HttpResponse { ContentType = "text/plain", Body = req.UserAgent });
router.AddRoute("/files/", req =>
{
    var segments = req.Path.Split('/');
    if (segments.Length < 3 || string.IsNullOrEmpty(segments[2]))
    {
        return new HttpResponse { StatusCode = "404 Not Found" };
    }
    
    if (string.IsNullOrEmpty(filesDirectory))
    {
        return new HttpResponse { StatusCode = "500 Internal Server Error", Body = "Directory not configured" };
    }
    
    var filename = segments[2];
    var fullPath = Path.Combine(filesDirectory, filename);

    if (!File.Exists(fullPath))
    {
        return new HttpResponse { StatusCode = "404 Not Found" };
    }
    
    var contents = File.ReadAllBytes(fullPath);
    return new HttpResponse 
    { 
        ContentType = "application/octet-stream", 
        BodyBytes = contents 
    };
});

var server = new TCPServer(router);
server.Start();