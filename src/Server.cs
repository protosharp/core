﻿using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading;

namespace OOPart
{
    public class Server
    {
        private HttpListener _listener;
        private readonly int _port;
        private Thread _thread;

        public Server(int port = 80)
        {
            this._port = port;
        }

        public void Start()
        {
            this._listener = new HttpListener();
            this._listener.Prefixes.Add($"http://*:{this._port}/");
            this._listener.Start();

            this._thread = new Thread(Listener);
            this._thread.Start();
        }

        private void Listener()
        {
            while(this._listener.IsListening)
            {
                AsyncCallback callback = new AsyncCallback(ListenerCallback);
                IAsyncResult result = this._listener.BeginGetContext(callback, this._listener);

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

            var path = Router.Parse(request);
            var context = new Context(request, response);
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
        }
    }
}
