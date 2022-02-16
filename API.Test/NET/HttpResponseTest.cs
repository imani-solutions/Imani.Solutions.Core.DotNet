using System;
using Imani.Solutions.Core.API.NET;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Imani.Solutions.Core.API.Test.NET
{
    [TestClass]
    public class HttpResponseTest
    {
        [TestMethod]
        public void When_ToString_Then_ContainsExpectedFields()
        {
            int expectedStatusCode = 200;
            string expectedBody = "Hello";
            var subject = new HttpResponse(expectedStatusCode,expectedBody);
            
            var actual = subject.ToString();

            Console.WriteLine(actual);
            Assert.IsTrue(actual.Contains(expectedStatusCode.ToString()));
            Assert.IsTrue(actual.Contains(expectedBody));
            
        }
        
    }
}