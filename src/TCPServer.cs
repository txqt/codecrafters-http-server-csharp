using System.Net;
using System.Net.Sockets;
using System.Text;

namespace codecrafters_http_server
{
    public class TCPServer
    {
        private TcpListener _tcpListener;
        private Router _router;
        public TCPServer(Router router)
        {
            _router = router;
        }

        public void Start(int? port = 4221)
        {
            if (port == null)
            {
                Console.WriteLine("Port is null");
                throw new Exception("Port is null");
            }
            
            _tcpListener = new TcpListener(IPAddress.Loopback, port.Value);
            _tcpListener.Start();
            Console.WriteLine($"Server is running at http://localhost:{port}/");

            while (true)
            {
                TcpClient client = _tcpListener.AcceptTcpClient();
                Task.Run(() => HandleClient(client));
            }
        }

        private void HandleClient(TcpClient client)
        {
            using var stream = client.GetStream();
            byte[] buffer = new byte[4096];
            int bytesRead = stream.Read(buffer, 0, buffer.Length);
            string requestText = Encoding.UTF8.GetString(buffer, 0, bytesRead);

            var request = new HttpRequest(requestText);
            var response = _router.Route(request);

            stream.Write(response.ToBytes(), 0, response.ToBytes().Length);
            client.Close();
        }
    }

}
