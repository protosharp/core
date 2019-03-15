using System.Net;

namespace oopart
{
    public class Context
    {
        public HttpListenerRequest Request {get; set;}
        public HttpListenerResponse Response {get; set;}
        public Context(HttpListenerRequest request, HttpListenerResponse response)
        {
            this.Request = request;
            this.Response = response;
            this.Response.Headers["Server"] = "oopart";
        }

        public void Send(byte[] bytes)
        {
            this.Response.ContentLength64 = bytes.Length;
            
            var stream = Response.OutputStream;
            stream.Write(bytes, 0, bytes.Length);
            
            stream.Close();
            this.Response.Close();
        }
    }
}