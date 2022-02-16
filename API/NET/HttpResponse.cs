using System.Text;

namespace Imani.Solutions.Core.API.NET
{
    public class HttpResponse
    {
        public HttpResponse()
        {
        }

        public HttpResponse(int statusCode, string body)
        {
            this.StatusCode = statusCode;
            this.Body = body;
        }
        public int StatusCode {get; set;} 
        public  string Body {get; set;}

        public override string ToString(){
            return $"HttpResponse[StatusCode:{StatusCode}, Body:{Body}]";
        }

    }
}