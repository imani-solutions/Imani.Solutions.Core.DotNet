using System;
using Imani.Solutions.Core.API.Util;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Imani.Solutions.Core.API.Test.Util
{
    [TestClass]
    public class CryptionTest
    {
        [TestMethod]
        public void cryption()
        {
            Verify("SECRETSAUCE", "Hello");
        }

        [TestMethod]
        public void cryption_when_different_size_ThenKeyAdjust()
        {
            Verify("1", "Hello1");
            Verify("123", "Hello2");
            Verify("123".PadRight(16), "Hello3");
            Verify("123".PadRight(32), "Hello4");
        }

        [TestMethod]
        public void When_TextEmpty_Then_ReturnArgument()
        {
            Assert.ThrowsException<ArgumentException>(() => Verify(null, "hello"));
        }


        private void Verify(string key, string expected)
        {
            Cryption subject = new Cryption(key);
            string actual = subject.EncryptText(expected);
            Assert.AreEqual(expected, subject.DecryptText(actual));

        }
    }
}