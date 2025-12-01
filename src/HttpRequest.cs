namespace codecrafters_http_server;

public class HttpRequest
{
    public string Method { get; private set; }
    public string Path { get; private set; }
    public string Version { get; private set; }
    public string Host { get; private set; }
    public string Accept { get; private set; }
    public string UserAgent { get; private set; }

    public HttpRequest(string requestText)
    {
        var lines = requestText.Split("\r\n");

        if (lines.Length > 0)
        {
            var statusLine = lines[0].Split(' ');
            if (statusLine.Length >= 3)
            {
                Method = statusLine[0];
                Path = statusLine[1];
                Version = statusLine[2];
            }
            else
            {
                Method = "";
                Path = "";
                Version = "";
            }
        }

        // Parse headers starting from the second line
        for (int i = 1; i < lines.Length; i++)
        {
            var line = lines[i];
            if (string.IsNullOrEmpty(line)) break; // End of headers

            var separatorIndex = line.IndexOf(':');
            if (separatorIndex != -1)
            {
                var headerName = line.Substring(0, separatorIndex).Trim();
                var headerValue = line.Substring(separatorIndex + 1).Trim();

                if (headerName.Equals("Host", StringComparison.OrdinalIgnoreCase))
                {
                    Host = headerValue;
                }
                else if (headerName.Equals("User-Agent", StringComparison.OrdinalIgnoreCase))
                {
                    UserAgent = headerValue;
                }
                else if (headerName.Equals("Accept", StringComparison.OrdinalIgnoreCase))
                {
                    Accept = headerValue;
                }
            }
        }
    }
}
