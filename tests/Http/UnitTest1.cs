using NUnit.Framework;
using OOPArt;

namespace Tests
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
            
        }

        [Test]
        public void Test1()
        {
            Assert.AreEqual(MIMEType.GetContentType("file.css"), "text/css");
        }

        [Test]
        public void Test2()
        {
            Assert.IsTrue(true);
        }
    }
}