using System.Collections.Generic;
using Imani.Solutions.Core.Util;
using Xunit;

namespace Imani.Solutions.Core.Test
{
    public class TextTest
    {
        Text subject = new Text();


        [Fact]
        public void Format()
        {
            IDictionary<string, string> map = new Dictionary<string, string>();
        
            map["firstName"] = "Gregory";
            map["lastName"] = "Green";

            var actual = subject.Format("${firstName} ${lastName}", map);

            Assert.Equal("Gregory Green",actual);

        }


        [Fact]
        public void Format_Removes_Extract_Special_Characters()
        {
            IDictionary<string, string> map = new Dictionary<string, string>();
        
            map["firstName"] = "Nyla";
            map["lastName"] = "Green";

            var actual = subject.Format("${firstName} ${lastName}${suffix}", map);

            Assert.Equal("Nyla Green",actual);

        }
       
        
        
    }
}