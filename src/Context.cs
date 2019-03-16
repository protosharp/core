using System.Net;
using System.Text;

namespace OOPart
{
    public class Context
    {
        private Encoding encoding;
        public HttpListenerRequest Request {get; set;}
        public HttpListenerResponse Response {get; set;}
        public Context(HttpListenerRequest request, HttpListenerResponse response)
        {
            this.encoding = Encoding.UTF8;
            this.Request = request;
            this.Response = response;
            this.Response.Headers["Server"] = "OOPart";
        }

        public void Send(byte[] bytes)
        {
            this.Response.ContentLength64 = bytes.Length;
            
            var stream = this.Response.OutputStream;
            stream.Write(bytes, 0, bytes.Length);
            
            stream.Close();
            this.Response.Close();
        }

        public void SendFile(string filename, byte[] bytes)
        {
            var contentType = MIMEType.GetContentType(filename);
            this.Response.Headers.Add("Content-Type", contentType);

            this.Send(bytes);
        }

        public void SendText(string text)
        { 
            this.Response.Headers.Add("Content-Type", "text/plain");
            
            var bytes = this.encoding.GetBytes(text);
            this.Send(bytes);
        }
    }
}