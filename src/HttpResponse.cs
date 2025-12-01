using System.Text;

namespace codecrafters_http_server;

public class HttpResponse
{
    public string StatusCode { get; set; } = "200 OK";
    public string Body { get; set; } = "";
    public string ContentType { get; set; } = "text/html; charset=UTF-8";

    public byte[] ToBytes()
    {
        string response =
            $"HTTP/1.1 {StatusCode}\r\n" +
            $"Content-Type: {ContentType}\r\n" +
            $"Content-Length: {Encoding.UTF8.GetByteCount(Body)}\r\n" +
            "Connection: close\r\n\r\n" +
            Body;

        return Encoding.UTF8.GetBytes(response);
    }
}
