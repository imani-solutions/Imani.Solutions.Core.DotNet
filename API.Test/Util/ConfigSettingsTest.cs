using System;
using Imani.Solutions.Core.API.Util;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Imani.Solutions.Core.API.Test.Util
{
    [TestClass]
    public class ConfigSettingsTest
    {
        private ConfigSettings  subject = new ConfigSettings();
        string expected_KAFKA_CONSUMER_TOPIC = "in";
        string expected_KAFKA_BOOTSTRAP_SERVERS = "servers";
        string expected_KAFKA_CONSUMER_GROUP = "group";
        string expected_KAFKA_PRODUCER_TOPIC = "output";

        string KAFKA_CONSUMER_TOPIC_CONF = "spring.cloud.stream.bindings.input.destination";
        string KAFKA_BOOTSTRAP_SERVERS_CONF = "SPRING_CLOUD_STREAM_KAFKA_BINDER_BROKERS";
        string KAFKA_CONSUMER_GROUP_CONF = "spring.cloud.stream.bindings.input.group";
        string KAFKA_PRODUCER_TOPIC_CONFIG = "spring.cloud.stream.bindings.output.destination";


        [TestInitialize]
        public void InitializeProgramTest()
        {

            Environment.SetEnvironmentVariable(KAFKA_CONSUMER_TOPIC_CONF, expected_KAFKA_CONSUMER_TOPIC);
            Environment.SetEnvironmentVariable(KAFKA_BOOTSTRAP_SERVERS_CONF, expected_KAFKA_BOOTSTRAP_SERVERS);
            Environment.SetEnvironmentVariable(KAFKA_CONSUMER_GROUP_CONF, expected_KAFKA_CONSUMER_GROUP);
            Environment.SetEnvironmentVariable(KAFKA_PRODUCER_TOPIC_CONFIG, expected_KAFKA_PRODUCER_TOPIC);

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
            Assert.AreEqual("A_B",subject.FormatEnvVarName("A.B"));
        }


        [TestMethod]
        public void FormatEnvVarName_caseinsensitve()
        {
            Assert.AreEqual("A_B",subject.FormatEnvVarName("a.b"));
        }
        [TestMethod]
        public void FormatEnvVarName_WhenEmpty()
        {
            Assert.IsNull(subject.FormatEnvVarName(""));
        }

    }
}