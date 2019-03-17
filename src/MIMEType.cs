using System;

namespace OOPArt
{
    public static class MIMEType
    {
        private static Func<string, string, bool> IsExtension = (string filename, string extension) => filename.EndsWith(extension, StringComparison.InvariantCultureIgnoreCase);
        public static string GetContentType(string filename)
        {
            if(IsExtension(filename, ".css"))
            {
                return "text/css";
            }

            if(IsExtension(filename, ".js"))
            {
                return "application/x-javascript";
            }

            if(IsExtension(filename, ".html"))
            {
                return "text/html";
            }

            return "text/plain";
        }
    }
}