using System;
using System.IO;
using System.Net;
using System.Text;

namespace Imani.Solutions.Core.API.NET
{
    public class Http : IHttp
    {
        private WebHeaderCollection requestHeaders = new WebHeaderCollection();
        private string ResponseHeaders = null;
        private readonly Encoding encode = System.Text.Encoding.UTF8;

        private string username;
        private char[] password;
        private string domain;
        private string encodedCredential;
        private readonly ICredentials networkCredential;

        public Http()
        { }
        public Http(string username, char[] password, string domain)
        {
            this.username = username;
            this.password = password;
            this.domain = domain;

            if(username != null && username.Length > 0)
            {
                this.networkCredential = new NetworkCredential(username, new string(password), domain);
                string usernameAndPassword = string.Concat(username, ":", new string(password));
                this.encodedCredential = Convert.ToBase64String(Encoding.ASCII.GetBytes(usernameAndPassword));
            }

        }//------------------------------------------------------


        public static bool IsOK(string url)
        {
            try
            {
                WebResponse result = null;
                WebRequest req = WebRequest.Create(url);
                req.Method = "GET";

                result = req.GetResponse();

                return true;
            }
            catch
            {
                return false;
            }

        }//------------------------------------------------------
        /// <summary>
        /// Pre
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public HttpResponse Get(string url)
        {

            ResponseHeaders = null;
            String responseSB;
            HttpWebResponse result = null;
            int resultCode = 200;
            try
            {
                WebRequest req = WebRequest.Create(url);

                if(this.networkCredential != null )
                    req.Credentials = networkCredential;

                req.Method = "GET";
                result = (HttpWebResponse)req.GetResponse();
                
                if (result != null)
                    resultCode = (int)result.StatusCode;


                StringBuilder headers = new StringBuilder();

                foreach (string header in result.Headers.AllKeys)
                {
                    headers.Append(" header=" + header + " value=" + result.Headers[header])
                        .Append(Environment.NewLine);
                }

                this.ResponseHeaders = headers.ToString();

                using (Stream ReceiveStream = result.GetResponseStream())
                {
                    using(StreamReader sr = new StreamReader(ReceiveStream, encode))
                    {
                        responseSB = sr.ReadToEnd();
                    }
                }

            }
            catch (WebException e)
            {
                throw new WebException($"URL {url} ERROR:{e}");
            }

            return new HttpResponse(resultCode,responseSB);


        }//-------------------------------------------------------
        /// <summary>
        /// Read the text response for a URL
        /// </summary>
        /// <param name="url"></param>
        /// <param name="method"></param>
        /// <returns></returns>
        public TextReader ReadText(String url, string method)
        {

            try
            {
                ResponseHeaders = null;
                StringBuilder responseSB = new StringBuilder(1024);
                WebResponse result = null;

                WebRequest req = WebRequest.Create(url);
                if(this.networkCredential != null)
                    req.Credentials = this.networkCredential;

                req.Method = method;
                result = req.GetResponse();

                //result = req.GetResponse();
                StringBuilder headers = new StringBuilder();

                foreach (string header in result.Headers.AllKeys)
                {
                    headers.Append(" header=" + header + " value=" + result.Headers[header])
                        .Append(Environment.NewLine);
                }

                this.ResponseHeaders = headers.ToString();

                using(Stream receiveStream = result.GetResponseStream())
                {
                    return new StreamReader(receiveStream, encode);
                }

            }
            catch (UriFormatException e)
            {
                throw new UriFormatException(url + " " + e.StackTrace);
            }
        }//-------------------------------------------------------

        public HttpResponse Put(string url, string payload, string contentType)
        {
            return Invoke("PUT", url, payload, contentType);
        }

        public HttpResponse Post(string url, string payload, string contentType)
        {
            return Invoke("POST", url, payload, contentType);
        }
        public HttpResponse Delete(string url)
        {
            return Invoke("DELETE", url, null, null);
        }


        public HttpResponse Invoke(string method, string url, string payload, string contentType)
        {
            ResponseHeaders = null;
            int resultCode = 0;

            String responseSB;
            try
            {
                //add headers
                WebRequest req = WebRequest.Create(url);

                if (this.requestHeaders.Count > 0)
                {
                    string[] keys = requestHeaders.AllKeys;
                    string key = null;
                    for (int i = 0; i < keys.Length; i++)
                    {
                        key = keys[i];
                        req.Headers.Add(key, requestHeaders.Get(key));
                    }//end for

                }

                if(this.networkCredential != null)
                {
                    req.Credentials = this.networkCredential;
                
                    req.Headers.Add("Authorization", "Basic " + encodedCredential);

                }


                req.Method = method;
                req.ContentType = contentType;
                StringBuilder UrlEncoded = new StringBuilder();

                Char[] reserved = { '?', '=', '&' };
                byte[] SomeBytes = null;

                if (payload != null)
                {

                    //SomeBytes = Encoding.UTF8.GetBytes(UrlEncoded.ToString());
                    SomeBytes = Encoding.UTF8.GetBytes(payload);
                    req.ContentLength = SomeBytes.Length;
                    using(Stream newStream = req.GetRequestStream())
                    {
                        newStream.Write(SomeBytes, 0, SomeBytes.Length);
                    }
                }
                else
                {
                    req.ContentLength = 0;
                }


                using (HttpWebResponse result = (HttpWebResponse)req.GetResponse())
                {
                    StringBuilder headers = new StringBuilder();

                    foreach (string header in result.Headers.AllKeys)
                    {
                        headers.Append(" header=" + header + " value=" + result.Headers[header])
                            .Append(Environment.NewLine);
                    }

                    this.ResponseHeaders = headers.ToString();

                    Stream ReceiveStream = result.GetResponseStream();
                    StreamReader sr = new StreamReader(ReceiveStream, encode);
                    responseSB = sr.ReadToEnd();
                    resultCode = (int)result.StatusCode;
                }

            }
            catch (WebException e)
            {
                string error = $"ERROR:{e} method:{method} url:{url} payload:{payload} contentType:{contentType}";

                throw new WebException(error,e);
            }

            return new HttpResponse(resultCode,responseSB);
        }


        public void AddHeader(string name, string value)
        {
            requestHeaders.Add(name, value);
        }
    }
}