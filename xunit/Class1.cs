using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace xunit
{
    public class Class1
    {
        [Fact]
        public void xunit_test1(){
            Assert.Throws(typeof(Exception), () => {
                throw new Exception();
            });
            Assert.True(true);
        }
    }
}
