using System;
using System.Linq;
using System.Net;
using System.Reflection;

namespace ProtoSharp
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
    }
}