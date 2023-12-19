using System.Net;
using System.Text;

internal class Program
{
    private static void Main(string[] args)
    {
        Console.WriteLine("Hello, World!");

        var listener = new HttpListener();
        listener.Prefixes.Add(new UriBuilder(Uri.UriSchemeHttp, "*", 1123).ToString());

        if (!HttpListener.IsSupported)
        {
            Console.WriteLine("http listener is not supported");
            return;
        }
        listener.Start();

        Console.WriteLine("listening on [{0}]", string.Join(',', listener.Prefixes));
        var httpClient = new HttpClient();
        var publicIp = httpClient.GetStringAsync("https://api.ipify.org").Result;
        httpClient.Dispose();

        Console.WriteLine("public ip is [{0}]", publicIp);


        while (true)
        {
            var context = listener.GetContext();
            var request = context.Request;
            var response = context.Response;

            if (request != null)
            {
                Console.WriteLine("request received from [{0}]", default);
                Console.WriteLine("request method [{0}]", request?.HttpMethod);
                Console.WriteLine("request url [{0}]", request?.Url);
                Console.WriteLine("request headers [{0}]", string.Join(',', request?.Headers.AllKeys.Select(k => $"{k}={request.Headers[k]}") ?? Array.Empty<string>()));
            }

            var responseString = "Hello, World!";
            var buffer = Encoding.UTF8.GetBytes(responseString);
            response.ContentLength64 = buffer.Length;
            response.OutputStream.Write(buffer, 0, buffer.Length);
            response.OutputStream.Close();
        }
    }
}