using System.Net;
using System.Net.Sockets;
using System.Text;

namespace codecrafters_http_server
{
    public class TCPServer
    {
        private TcpListener _tcpListener;

        public void Start()
        {
            var port = 4221;
            var hostAddress = IPAddress.Loopback;
            _tcpListener = new TcpListener(hostAddress, port);
            _tcpListener.Start();
            Console.WriteLine("Server is running at http://localhost:4221/");

            while (true)
            {
                TcpClient client = _tcpListener.AcceptTcpClient();
                Console.WriteLine("Client connected!");

                NetworkStream stream = client.GetStream();

                byte[] buffer = new byte[4096];
                int bytesRead = stream.Read(buffer, 0, buffer.Length);
                string requestText = Encoding.UTF8.GetString(buffer, 0, bytesRead);
                Console.WriteLine("Request received:");
                Console.WriteLine(requestText);

                string requestLine = requestText.Split("\r\n")[0];

                var parser = new HttpRequestParser(requestText);

                string statusLine;
                string body;

                if (parser.Path == "/")
                {
                    statusLine = "HTTP/1.1 200 OK\r\n";
                    body = "<h1>Hello from TcpListener!</h1>";
                }
                else
                {
                    statusLine = "HTTP/1.1 404 Not Found\r\n";
                    body = "<h1>Not Found</h1>";
                }

                string response =
                    statusLine +
                    "Content-Type: text/html; charset=UTF-8\r\n" +
                    $"Content-Length: {Encoding.UTF8.GetByteCount(body)}\r\n" +
                    "Connection: close\r\n" +
                    "\r\n" +
                    body;

                byte[] responseBytes = Encoding.UTF8.GetBytes(response);
                stream.Write(responseBytes, 0, responseBytes.Length);

                client.Close();
            }
        }
    }
}
