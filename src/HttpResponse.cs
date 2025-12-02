using System.Text;

namespace codecrafters_http_server;

public class HttpResponse
{
    public string StatusCode { get; set; } = "200 OK";
    public string Body { get; set; } = "";
    public byte[]? BodyBytes { get; set; } = null;
    public string ContentType { get; set; } = "text/html; charset=UTF-8";

    public byte[] ToBytes()
    {
        // Use BodyBytes if available, otherwise use Body string
        byte[] bodyContent = BodyBytes ?? Encoding.UTF8.GetBytes(Body);
        
        string headers =
            $"HTTP/1.1 {StatusCode}\r\n" +
            $"Content-Type: {ContentType}\r\n" +
            $"Content-Length: {bodyContent.Length}\r\n" +
            "Connection: close\r\n\r\n";

        byte[] headerBytes = Encoding.UTF8.GetBytes(headers);
        byte[] response = new byte[headerBytes.Length + bodyContent.Length];
        
        Buffer.BlockCopy(headerBytes, 0, response, 0, headerBytes.Length);
        Buffer.BlockCopy(bodyContent, 0, response, headerBytes.Length, bodyContent.Length);

        return response;
    }
}
