namespace codecrafters_http_server;

public class HttpRequest
{
    public string Method { get; private set; }
    public string Path { get; private set; }
    public string Version { get; private set; }

    public HttpRequest(string requestText)
    {
        string requestLine = requestText.Split("\r\n")[0];

        string[] parts = requestLine.Split(' ');

        if (parts.Length >= 3)
        {
            Method = parts[0];
            Path = parts[1];
            Version = parts[2];
        }
        else
        {
            Method = "";
            Path = "";
            Version = "";
        }
    }
}
