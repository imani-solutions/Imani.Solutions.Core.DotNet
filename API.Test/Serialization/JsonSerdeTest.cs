using Imani.Solutions.Core.API.Serialization;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Imani.Solutions.Core.API.Test.Serialization
{
    [TestClass]
    public class JsonSerdeTest
    {

         private JsonSerde<DomainQa> subject;
         
         [TestInitialize]
         public void InitializeJsonSerdeTest()
         {
                 subject = new JsonSerde<DomainQa>();
         }

        [TestMethod]
        public void Serializization()
        {
            DomainQa expected = new DomainQa(){
                Id = "hello"
            };
           
            var actualSerialized = subject.Serialize(expected);
            Assert.IsNotNull(actualSerialized);

            var actual = subject.Deserialize(actualSerialized);
            Assert.AreEqual(expected.Id,actual.Id);
            
        }

        [TestMethod]
        public void Deserialize()
        {
            string json ="{\"Id\" : \"You\"}";
            DomainQa actual = subject.Deserialize(json);
            Assert.AreEqual("You",actual.Id);
        }

          [TestMethod]
        public void Deserialize_Empty()
        {
            string json ="{}";
            DomainQa actual = subject.Deserialize(json);
            Assert.IsNotNull(actual);
        }
        
    }
       
}