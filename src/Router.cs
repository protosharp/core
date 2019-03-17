using System;
using System.Linq;
using System.Net;
using System.Reflection;

namespace OOPArt
{
    public static class Router
    {
        public static object Call(string functionName, string methodName, object[] parameters)
        {
            var types = Assembly.GetEntryAssembly().GetTypes().Concat(Assembly.GetExecutingAssembly().GetTypes());

            foreach(var type in types)
            {
                if(type.Name.EndsWith(functionName, StringComparison.InvariantCultureIgnoreCase))
                {
                    foreach(var method in type.GetMethods())
                    {
                        if(method.Name.EndsWith(methodName, StringComparison.InvariantCultureIgnoreCase))
                        {
                            var instance = Activator.CreateInstance(type);
                            
                            return method.Invoke(instance, parameters);
                        }
                    }
                }
            }

            return null;
        }

        public static string[] Parse(HttpListenerRequest request)
        {
            var splitted = request.Url.Segments
                .Select(s => s.ToLower().Replace("/", ""))
                .Where(w => w.Length > 0)
                .ToArray<string>();
            
            if (IsFilename(request.Url.LocalPath))
            {
                return new [] {splitted.Last() };
            }

            string controller = "Home", action = "Index";

            if(splitted.Length >= 2)
            {
                controller = splitted[0];
                action = splitted[1];
            }

            return new [] { controller, action }.Concat(splitted.Skip(2)).ToArray<string>();
        }

        private static bool IsFilename(string url)
        {
            return url.Contains('.');
        }
        
    }
}