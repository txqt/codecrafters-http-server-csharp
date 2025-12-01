using System.Net;
using System.Net.Sockets;
using System.Text;
using codecrafters_http_server;

// You can use print statements as follows for debugging, they'll be visible when running tests.
Console.WriteLine("Logs from your program will appear here!");

var router = new Router();
router.AddRoute("/", req => new HttpResponse { Body = "<h1>Hello</h1>" });
router.AddRoute("/echo/", req => new HttpResponse { ContentType = "text/plain", Body = req.Path.Split('/')[2] });
router.AddRoute("/user-agent", req => new HttpResponse { ContentType = "text/plain", Body = req.UserAgent });

var server = new TCPServer(router);
server.Start();