namespace Imani.Solutions.Core.API.NET
{
    public interface IHttp
    {
        HttpResponse Get(string url);
        HttpResponse Put( string url, string payload, string contentType);
        HttpResponse Post( string url, string payload, string contentType);
        HttpResponse Delete(string url);

        HttpResponse Invoke(string method, string url, string payload, string contentType);

    }
}