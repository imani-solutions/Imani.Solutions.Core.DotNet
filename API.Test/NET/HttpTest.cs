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

            var html = subject.Get(url);
            Console.WriteLine("HTML:"+html);
            Assert.IsNotNull(html);

      }
  }
}