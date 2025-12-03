namespace codecrafters_http_server;

public class HttpRequest
{
    public string Method { get; private set; }
    public string Path { get; private set; }
    public string Version { get; private set; }
    public string Host { get; private set; }
    public string Accept { get; private set; }
    public string UserAgent { get; private set; }
    public int ContentLength { get; private set; }
    public byte[] BodyBytes { get; private set; }

    public HttpRequest(byte[] rawRequest, int length)
    {
        Method = "";
        Path = "";
        Version = "";
        Host = "";
        Accept = "";
        UserAgent = "";
        ContentLength = 0;
        BodyBytes = Array.Empty<byte>();

        // Find the end of headers (position of "\r\n\r\n")
        int headerEndIndex = FindHeaderEnd(rawRequest, length);
        
        // Parse headers section
        string headerText = System.Text.Encoding.UTF8.GetString(rawRequest, 0, headerEndIndex);
        ParseHeaders(headerText);
        
        // Extract body if Content-Length is specified
        int bodyStartIndex = headerEndIndex + 4; // Skip "\r\n\r\n"
        int availableBodyLength = length - bodyStartIndex;
        
        if (ContentLength > 0 && availableBodyLength > 0)
        {
            int actualBodyLength = Math.Min(ContentLength, availableBodyLength);
            BodyBytes = new byte[actualBodyLength];
            Array.Copy(rawRequest, bodyStartIndex, BodyBytes, 0, actualBodyLength);
        }
    }

    private int FindHeaderEnd(byte[] data, int length)
    {
        // Find the sequence: \r\n\r\n (bytes: 13, 10, 13, 10)
        for (int i = 0; i < length - 3; i++)
        {
            if (data[i] == 13 && data[i + 1] == 10 && 
                data[i + 2] == 13 && data[i + 3] == 10)
            {
                return i;
            }
        }
        return length;
    }

    private void ParseHeaders(string headerText)
    {
        var lines = headerText.Split("\r\n");

        if (lines.Length > 0)
        {
            var statusLine = lines[0].Split(' ');
            if (statusLine.Length >= 3)
            {
                Method = statusLine[0];
                Path = statusLine[1];
                Version = statusLine[2];
            }
        }

        // Parse headers starting from the second line
        for (int i = 1; i < lines.Length; i++)
        {
            var line = lines[i];
            if (string.IsNullOrEmpty(line)) break;

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
                else if (headerName.Equals("Content-Length", StringComparison.OrdinalIgnoreCase))
                {
                    int.TryParse(headerValue, out int contentLength);
                    ContentLength = contentLength;
                }
            }
        }
    }
}
