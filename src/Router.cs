namespace codecrafters_http_server;

public class Router
{
    private readonly Dictionary<string, Func<HttpRequest, HttpResponse>> _routes =
        new Dictionary<string, Func<HttpRequest, HttpResponse>>();

    public void AddRoute(string path, Func<HttpRequest, HttpResponse> handler)
    {
        _routes[path] = handler;
    }

    public HttpResponse Route(HttpRequest request)
    {
        foreach (var route in _routes)
        {
            // Exact match for root path "/"
            if (route.Key == "/" && request.Path == "/")
                return route.Value(request);
            
            // Prefix match for other routes (but not for root)
            if (route.Key != "/" && request.Path.StartsWith(route.Key))
                return route.Value(request);
        }

        return new HttpResponse { StatusCode = "404 Not Found", Body = "<h1>Not Found</h1>" };
    }
}
