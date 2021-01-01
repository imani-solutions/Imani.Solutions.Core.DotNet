using System;
using Imani.Solutions.Core.API.Util;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Imani.Solutions.Core.API.Test.Util
{
    /// <summary>
    /// Testing for cryption 
    /// 
    /// author: Gregory Green
    /// </summary>
    [TestClass]
    public class CryptionTest
    {
        private string key = "24232323";
        private Cryption subject;

        [TestInitialize]
        public void InitializeCryptionTest()
        {
            subject = new Cryption(key);
        }

        [TestMethod]
        public void cryption()
        {
            Verify("SECRETSAUCE", "Hello");
        }

        [TestClass]
        public class InterruptTest
        {

            private string key = "24232323";
            private Cryption subject;
            private string value = "PASSWORD";

            [TestInitialize]
            public void InitializeCryptionTest()
            {
                subject = new Cryption(key);
            }


            [TestMethod]
            public void Interrupt()
            {

                Assert.AreEqual(value, subject.Interrupt(value));

            }


            [TestMethod]
            public void Interrupt_WhenNullOrEmpty_ThenReturnEmty()
            {

                Assert.AreEqual("", subject.Interrupt(null));
                Assert.AreEqual("", subject.Interrupt(""));

            }

            [TestMethod]
            public void EncryptTextWithPrefix()
            {

                String encryptedPasswordWithPrefix = subject.EncryptTextWithPrefix(value);
                Assert.IsTrue(encryptedPasswordWithPrefix.StartsWith(Cryption.CRYPTION_PREFIX));
            }

            [TestMethod]
            public void Interrupt_WithPrefix()
            {
                String encryptedPasswordWithPrefix = subject.EncryptTextWithPrefix(value);
                Assert.AreEqual(value, subject.Interrupt(encryptedPasswordWithPrefix));

            }
            [TestMethod]
            public void Interrupt_WithWhiteSpaceBeforePrefix()
            {
                String encryptedPasswordWithPrefix = " " + subject.EncryptTextWithPrefix(value);
                Assert.AreEqual(value, subject.Interrupt(encryptedPasswordWithPrefix));

            }
            [TestMethod]
            public void Interrupt_WithTabSpaceBeforePrefix()
            {
                String encryptedPasswordWithPrefix = "\t" + subject.EncryptTextWithPrefix(value);
                Assert.AreEqual(value, subject.Interrupt(encryptedPasswordWithPrefix));

            }
        }

        [TestMethod]
        public void Cryption_when_different_size_ThenKeyAdjust()
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