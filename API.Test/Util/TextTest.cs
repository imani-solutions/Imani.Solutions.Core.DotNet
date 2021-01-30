using System;
using System.Collections.Generic;
using Imani.Solutions.Core.API.Util;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Imani.Solutions.Core.API.Test.Util
{

    /// <summary>
    /// Testing for Texting 
    /// 
    /// author: Gregory Green
    /// </summary>
    [TestClass]
    public class TextTest
    {
        Text subject = new Text();


        [TestMethod]
        public void Format()
        {
            IDictionary<string, string> map = new Dictionary<string, string>();
        
            map["firstName"] = "Gregory";
            map["lastName"] = "Green";

            var actual = subject.Format("${firstName} ${lastName}", map);

            Assert.AreEqual("Gregory Green",actual);

        }


        [TestMethod]
        public void Format_Removes_Extract_Special_Characters()
        {
            IDictionary<string, string> map = new Dictionary<string, string>();
        
            map["firstName"] = "Nyla";
            map["lastName"] = "Green";

            var actual = subject.Format("${firstName} ${lastName}${suffix}", map);

            Assert.AreEqual("Nyla Green",actual);

        }
       
        [TestMethod]
        public void GenerateText()
        {
            int expectedSize = 10;
            string actual = subject.GenerateText(expectedSize,"hello");
            Console.WriteLine($"OUT[{actual}]"); 
            Assert.IsNotNull(actual);
            Assert.AreEqual(expectedSize,actual.Length);

            Assert.AreEqual("",subject.GenerateText(0,"a"));


            Assert.ThrowsException<ArgumentException>( 
                () => subject.GenerateText(1,""));

            Assert.ThrowsException<ArgumentException>( 
                () => subject.GenerateText(0,""));
            

            Assert.ThrowsException<ArgumentException>( 
                () => subject.GenerateText(1,null));

            Assert.ThrowsException<ArgumentException>( 
                () => subject.GenerateText(0,null));
        }
        
    }
}