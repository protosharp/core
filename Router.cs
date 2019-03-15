using System.Linq;
using System.Net;

namespace oopart
{
    public static class Router
    {
        public static string[] Parse(HttpListenerRequest request)
        {
            var splitted = request.Url.Segments
                .Select(s => s.ToLower().Replace("/", ""))
                .Where(w => w.Length > 0)
                .ToArray<string>();
            
            if (IsFilename(request.Url.LocalPath))
            {
                return new string[] {splitted.Last() };
            }

            return new string[] { };
        }

        private static bool IsFilename(string url)
        {
            return url.Contains('.');
        }
        
    }
}