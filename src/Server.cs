using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading;

namespace OOPArt
{
    public class Server
    {
        private readonly HttpListener _listener;
        private readonly int _port;
        private readonly Thread _thread;

        public Server(int port = 80)
        {
            this._port = port;

            this._listener = new HttpListener();
            this._thread = new Thread(Listener);
        }

        public void Start()
        {
            this._listener.Prefixes.Add($"http://*:{this._port}/");

            this._listener.Start();
            this._thread.Start();
        }

        private void Listener()
        {
            while(this._listener.IsListening)
            {
                AsyncCallback callback = new AsyncCallback(ListenerCallback);
                IAsyncResult result = this._listener.BeginGetContext(callback, this._listener);

                Console.WriteLine("Server running....");
                result.AsyncWaitHandle.WaitOne();
            }
        }

        private void ListenerCallback(IAsyncResult result)
        {
            try
            {
                var state = (HttpListener)result.AsyncState;
                var context = state.EndGetContext(result);
                var request = context.Request;
                var response = context.Response;

                this.HandleClient(request, response);
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private void HandleClient(HttpListenerRequest request, HttpListenerResponse response)
        {
            Console.WriteLine("Client handle...");

            var context = new Context(request, response);

            var path = context.ParseUrl();
            var url = "public/" + path.First();

            if (path.Length == 1)
            {
                if(! File.Exists(url))
                {
                    //context.Error404();
                    return;
                }

                context.SendFile(url, File.ReadAllBytes(url));
                return;
            }

            if(path.Length == 3)
            {
                string area = path[0];
                string functionName = path[1];
                string methodName = path[2];
                
                var body = context.ReadBody();
                var form = context.ParseBody(body, request.ContentType);
                

                if(area.Equals("Functions", StringComparison.InvariantCultureIgnoreCase))
                {
                    var parameters = context.ParseParameters(form);
                    
                    var result = Router.Call(functionName, methodName, parameters);
                    context.SendJson(result);
                    return;
                }

                if(area.Equals("Models", StringComparison.InvariantCultureIgnoreCase))
                {
                    var result = Router.Call(functionName, methodName, null);
                    context.SendJson(result);
                    return;
                }

            }
        }
    }
}
