using System;
using Imani.Solutions.Core.API.Util;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Imani.Solutions.Core.API.Test.Util
{
    /// <summary>
    /// Testing for config settings
    /// 
    /// author: Gregory Green
    /// </summary>
    [TestClass]
    public class ConfigSettingsTest
    {
        private ConfigSettings subject;
        private string expected_KAFKA_CONSUMER_TOPIC = "in";
        private string expected_KAFKA_BOOTSTRAP_SERVERS = "servers";
        private string expected_KAFKA_CONSUMER_GROUP = "group";
        private string expected_KAFKA_PRODUCER_TOPIC = "output";

        private string KAFKA_CONSUMER_TOPIC_CONF = "spring.cloud.stream.bindings.input.destination";
        private string KAFKA_BOOTSTRAP_SERVERS_CONF = "SPRING_CLOUD_STREAM_KAFKA_BINDER_BROKERS";
        private string KAFKA_CONSUMER_GROUP_CONF = "spring.cloud.stream.bindings.input.group";
        private string KAFKA_PRODUCER_TOPIC_CONFIG = "spring.cloud.stream.bindings.output.destination";


        [TestInitialize]
        public void InitializeProgramTest()
        {

            Environment.SetEnvironmentVariable(KAFKA_CONSUMER_TOPIC_CONF, expected_KAFKA_CONSUMER_TOPIC);
            Environment.SetEnvironmentVariable(KAFKA_BOOTSTRAP_SERVERS_CONF, expected_KAFKA_BOOTSTRAP_SERVERS);
            Environment.SetEnvironmentVariable(KAFKA_CONSUMER_GROUP_CONF, expected_KAFKA_CONSUMER_GROUP);
            Environment.SetEnvironmentVariable(KAFKA_PRODUCER_TOPIC_CONFIG, expected_KAFKA_PRODUCER_TOPIC);

            Environment.SetEnvironmentVariable("CRYPTION_KEY", "xQwdSd23sdsd23");

            subject = new ConfigSettings();
        }


        [TestMethod]
        public void GetConfigProp_throwsExceptionWhenMissing()
        {
            Environment.SetEnvironmentVariable("KAFKA_BOOTSTRAP_SERVERS", null);

            Assert.ThrowsException<ArgumentException>
            (() => subject.GetProperty("KAFKA_BOOTSTRAP_SERVERS"));
        }

        [TestMethod]
        public void GetProperty_Default()
        {
            string expected = "expected";
            Assert.AreEqual(expected, this.subject.GetProperty("INVAID", expected));

        }

        [TestClass]
        public class PasswordTest
        {
            private ConfigSettings subject;

            [TestInitialize]
            public void InitializeConfigSettingsTest()
            {

                subject = new ConfigSettings();
            }

            [TestMethod]
            public void GetPassword()
            {
                char[] expected = "secret".ToCharArray();
                var encrypted = subject.EncryptPassword(expected);
                Environment.SetEnvironmentVariable("MYPASSWORD", encrypted);
                char[] password = this.subject.GetPropertyPassword("MYPASSWORD");

                Assert.AreEqual(new string(expected), new string(password));

            }

            [TestMethod]
            public void GetSecret()
            {
                string expected = "secret";
                string encrypted = subject.EncryptSecret(expected);
                Environment.SetEnvironmentVariable("MYSECRET", encrypted);
                string secret = this.subject.GetPropertySecret("MYSECRET");

                Assert.AreEqual(expected, secret);
            }

            [TestMethod]
            public void GetSecret_Default()
            {
                string expected = "secret";
                string secret = this.subject.GetPropertySecret("DOESNOT_EXISTS", expected);

                Assert.AreEqual(expected, secret);
            }

            [TestMethod]
            public void GetSecret_Default_NotUsed()
            {
                string expected = "secret";
                var myKey = "sadaasda";
                Environment.SetEnvironmentVariable("CRYPTION_KEY", myKey);
                string encrypted = subject.EncryptSecret(expected);
                Environment.SetEnvironmentVariable("MYSECRET", encrypted);
                string secret = this.subject.GetPropertySecret("MYSECRET", "");

                Assert.AreEqual(expected, secret);
            }

            [TestMethod]
            public void GetPassword_Default()
            {

                Assert.AreEqual("EXPECTED", new string(subject.GetPropertyPassword("NEWPASSWORD", "EXPECTED".ToCharArray())));

            }
            [TestMethod]
            public void GetPasswordUnEncrypted()
            {
                string expected = "secret";
                Environment.SetEnvironmentVariable("MYPASSWORD", expected);
                char[] password = this.subject.GetPropertyPassword("MYPASSWORD");

                Assert.AreEqual(expected, new string(password));

            }
        }

        [TestMethod]
        public void GetConfigProp_DotReplaced_Equals()
        {
            string expectedEnvName = "GetConfigProp_DotReplaced_Equals";

            string expectedValue = "hello";
            Environment.SetEnvironmentVariable(expectedEnvName, expectedValue);


            Assert.AreEqual(subject.GetProperty(expectedEnvName),
            subject.GetProperty(expectedEnvName));
        }



        [TestMethod]
        public void LoadConfig_UsingArgs()
        {


            Environment.SetEnvironmentVariable(KAFKA_CONSUMER_TOPIC_CONF, null);
            Environment.SetEnvironmentVariable(KAFKA_BOOTSTRAP_SERVERS_CONF, null);
            Environment.SetEnvironmentVariable(KAFKA_CONSUMER_GROUP_CONF, null);
            Environment.SetEnvironmentVariable(KAFKA_PRODUCER_TOPIC_CONFIG, null);

            string[] args = {$"--{KAFKA_CONSUMER_TOPIC_CONF}={expected_KAFKA_CONSUMER_TOPIC}",
            $"--{KAFKA_BOOTSTRAP_SERVERS_CONF}={expected_KAFKA_BOOTSTRAP_SERVERS}",
            $"--{KAFKA_CONSUMER_GROUP_CONF}={expected_KAFKA_CONSUMER_GROUP}",
            $"--{KAFKA_PRODUCER_TOPIC_CONFIG}={expected_KAFKA_PRODUCER_TOPIC}"
            };


            subject = new ConfigSettings(args);

            Assert.AreEqual(subject.GetProperty(KAFKA_CONSUMER_TOPIC_CONF), expected_KAFKA_CONSUMER_TOPIC);
            Assert.AreEqual(subject.GetProperty(KAFKA_BOOTSTRAP_SERVERS_CONF), expected_KAFKA_BOOTSTRAP_SERVERS);
            Assert.AreEqual(subject.GetProperty(KAFKA_CONSUMER_GROUP_CONF), expected_KAFKA_CONSUMER_GROUP);
            Assert.AreEqual(subject.GetProperty(KAFKA_PRODUCER_TOPIC_CONFIG), expected_KAFKA_PRODUCER_TOPIC);
        }


        [TestMethod]
        public void GetConfigProp_ConsumerGroupfromSpring()
        {
            string envVar = "SPRING_CLOUD";
            string expected = "hello";

            Environment.SetEnvironmentVariable(envVar, expected);

            string actual = subject.GetProperty("Spring.Cloud");
            Assert.AreEqual(expected, actual);
        }
        [TestMethod]
        public void AddArgs_noArgs_doesNotThrowIndexOutOfRangeException()
        {
            string[] args = null;
            subject.AddArgs(args);

            string[] empty = { };
            subject.AddArgs(empty);

        }

        [TestMethod]
        public void AddArgs_invalidArgs()
        {

            string[] args = { "" };
            subject.AddArgs(args); //no exception


            string[] argsInValid = { "invalid" };
            subject.AddArgs(argsInValid);
            Assert.ThrowsException<ArgumentException>(() => subject.GetProperty(argsInValid[0]));



            string[] argsNotRight = { "=sd" };
            subject.AddArgs(argsNotRight);
            Assert.ThrowsException<ArgumentException>(() => subject.GetProperty("sd"));

        }

        [TestMethod]
        public void AddArgs()
        {
            string[] expecteds = { "expected0", "expected1" };
            string[] args = new string[expecteds.Length];

            for (int i = 0; i < expecteds.Length; i++)
            {
                args[i] = $"--spring.cloud{i}={expecteds[i]}";

            }

            subject.AddArgs(args);

            for (int i = 0; i < expecteds.Length; i++)
            {
                Assert.AreEqual(expecteds[i], subject.GetProperty($"spring_cloud{i}"));
            }


        }

        [TestMethod]
        public void FormatEnvVarName()
        {
            Assert.AreEqual("A_B", subject.FormatEnvVarName("A.B"));
        }

        [TestMethod]
        public void GetProperty_Integer()
        {
            Environment.SetEnvironmentVariable("PORT", "1");
            var actual = new ConfigSettings().GetPropertyInteger("PORT");
            Assert.AreEqual(1, actual);

        }


        [TestMethod]
        public void GetPropertyBoolean()
        {
            Environment.SetEnvironmentVariable("BOOL_PROP", "true");
            bool actual = new ConfigSettings().GetPropertyBoolean("BOOL_PROP");
            Assert.AreEqual(true, actual);

        }

        [TestMethod]
        public void GetPropertyBoolean_Default()
        {
            bool actual = new ConfigSettings().GetPropertyBoolean("BOOL_PROP_DOES_NOT_EXISTS", true);
            Assert.AreEqual(true, actual);

        }

        [TestMethod]
        public void GetProperty_Integer_Default()
        {

            var actual = new ConfigSettings().GetPropertyInteger("INVALID", 3);
            Assert.AreEqual(3, actual);

        }

        [TestMethod]
        public void FormatEnvVarName_caseinsensitve()
        {
            Assert.AreEqual("A_B", subject.FormatEnvVarName("a.b"));
        }
        [TestMethod]
        public void FormatEnvVarName_WhenEmpty()
        {
            Assert.IsNull(subject.FormatEnvVarName(""));
        }

    }
}