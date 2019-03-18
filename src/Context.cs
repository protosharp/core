using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using Newtonsoft.Json;

namespace OOPArt
{
    public class Context
    {
        private readonly Encoding _encoding;
        public HttpListenerRequest Request {get; set;}
        public HttpListenerResponse Response {get; set;}

        public Context(HttpListenerRequest request, HttpListenerResponse response)
        {
            this._encoding = Encoding.UTF8;
            this.Request = request;
            this.Response = response;
            this.Response.Headers["Server"] = "OOPArt";
        }

        public Dictionary<string, string> ParseBody(string body, string contentType)
        {
            if(contentType == "application/json")
            {
                return JsonConvert.DeserializeObject<Dictionary<string, string>>(body);
            }
            
            var form = new Dictionary<string, string>();

            var splitFields = new char[]{ '&' };
            var splitKeyValue = new char[]{ '=' };
            
            var fields = body.Split(splitFields);
            var keysValues = fields.Select(x => x.Split(splitKeyValue));

            foreach(var item in keysValues)
            {
                form.Add(item.First(), item.Last());
            }

            return form;
        }

        public object[] ParseParameters(Dictionary<string, string> form)
        {
            return new object[]{ int.Parse(form["a"]), int.Parse(form["b"])};
        }

        public string[] ParseUrl()
        {
            var splitted = this.Request.Url.Segments
                .Select(s => s.ToLower().Replace("/", ""))
                .Where(w => w.Length > 0)
                .ToArray<string>();
            
            if (this.Request.Url.LocalPath.Contains("."))
            {
                return new [] { splitted.Last() };
            }

            string controller = "Home", action = "Index";

            if(splitted.Length >= 2)
            {
                controller = splitted[0];
                action = splitted[1];
            }

            return new [] { controller, action }.Concat(splitted.Skip(2)).ToArray<string>();
        }

        public string ReadBody()
        {
            var stream = this.Request.InputStream;
            var streamReader = new StreamReader(stream);

            return streamReader.ReadToEnd();
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
            
            var bytes = this._encoding.GetBytes(text);
            this.Send(bytes);
        }
    }
}