using System;
using Imani.Solutions.Core.API.NET;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Imani.Solutions.Core.API.Test.NET
{
  [TestClass]
  public class HttpTest
  {
      private Http subject;

      [TestInitialize]
      public void InitializeHttpTest()
      {
          subject = new Http();
      }

      [TestMethod]
      public void get()
      {
            string url = "http://www.TheRevelationSquad.com";

            var httpResponse = subject.Get(url);
            Assert.IsNotNull(httpResponse);

            Assert.AreEqual(200,httpResponse.StatusCode);
            Console.WriteLine("HTML:"+httpResponse.Body);

      }

       public void post()
      {
            string url = "http://www.TheRevelationSquad.com";

            string payload = "{}";
            string contentType = "application/json";
            var httpResponse = subject.Post(url,payload,contentType);
            Assert.IsNotNull(httpResponse);
            
            Assert.AreEqual(200,httpResponse.StatusCode);
            Console.WriteLine("HTML:"+httpResponse.Body);

      }
  }
}