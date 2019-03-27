using NUnit.Framework;
using ProtoSharp;

namespace Tests.Http
{
    public class MIMETypeTest
    {
        [Test]
        public void TestMIMETypes()
        {
            Assert.AreEqual(MIMEType.GetContentType("file.css"), "text/css");
            Assert.AreEqual(MIMEType.GetContentType("file.js"), "application/x-javascript");
            Assert.AreEqual(MIMEType.GetContentType("file.html"), "text/html");

            Assert.AreEqual(MIMEType.GetContentType("file.unknow"), "text/plain");
        }
    }
}