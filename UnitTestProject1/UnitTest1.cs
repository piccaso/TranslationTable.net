using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using TranslationTable;

namespace UnitTestProject1 {
    
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    [TestClass]
    public class UnitTest1 : VW.VW {

        [TestMethod, ExpectedException(typeof(ReadOnlyException))]
        public void testA() {
            var tt = 
                    new SimpleTranslationTable()
                    .A("key", "value")
            ;
            Assert.IsTrue("value" == tt["key"]);
            var ro = new ReadonlySimpleTranslationTable<string>(tt);
            Assert.IsTrue("value" == ro["key"]);
            ro.A("x", "y"); // throws
            Assert.Fail();
        }

        [TestMethod, ExpectedException(typeof(InvalidNumberOfArgumentsException))]
        public void testAnoValue() {
            new SimpleTranslationTable().A("key");
            Assert.Fail();
        }

        [TestMethod, ExpectedException(typeof(InvalidNumberOfArgumentsException))]
        public void testAnoArgument() {
            new SimpleTranslationTable().A();
            Assert.Fail();
        }

        [TestMethod, ExpectedException(typeof(InvalidNumberOfArgumentsException))]
        public void testAworngLength() {
            new SimpleTranslationTable().A("k","v","k");
            Assert.Fail();
        }

        [TestMethod]
        public void vw() {
            if(isAgent) return;
            //Assert.Fail();
        }

        [TestMethod]
        public void TestMethod1() {
            var tt = new SimpleTranslationTable();
            tt["key"]="value";
            Assert.IsTrue(tt["key"]=="value");

            tt["key"]="xxx";
            Assert.IsTrue(tt["key"]=="xxx");

            tt["key"]=null;
            Assert.IsTrue(tt["key"]=="key");
        }


        [TestMethod, ExpectedException(typeof(ReadOnlyException))]
        public void TestMethod2() {
            var tt = new SimpleTranslationTable();
            tt["key"]="value";
            var ro = new ReadonlySimpleTranslationTable(tt);
            Assert.IsTrue("value"==ro["key"]);
            Assert.IsTrue("blah"==ro["blah"]);
            tt["key"]="xyz";
            Assert.IsTrue(ro["key"]=="xyz");
            ro["x"]="y"; /* throws */
            Assert.Fail();
        }

        [TestMethod]
        public void Test3() {
            var sd = new Dictionary<string, string>();
            var tt = new SimpleTranslationTable<string>(sd);
            var ro = new ReadonlySimpleTranslationTable<string>(tt);
            sd.Add("key", "value");
            tt
                .Add("key2", "value2")
                .Add("key3", "value3")
            ;
            Assert.IsTrue(sd["key3"]=="value3");
            Assert.IsTrue(ro["key3"]=="value3");
            Assert.IsTrue(tt["key3"]=="value3");

            Assert.IsTrue(sd["key"]=="value");
            Assert.IsTrue(ro["key"]=="value");
            Assert.IsTrue(tt["key"]=="value");

            var rod = ro.ToDictionary();
            var rox = new SimpleTranslationTable<string>(rod);
            rod.Add("blah", "blx");
            Assert.IsTrue(ro["blah", null]==null);
            Assert.IsTrue(rox["blah", null]=="blx");

        }


        [TestMethod]
        public void complexTypeA() {

            var tt = 
                new SimpleTranslationTable<myClass>()
                .A(new myClass { name="key1", age=1 }, new myClass { name="value1", age=1 })
                .A(new myClass { name="key1", age=1 }, new myClass { name="value1", age=1 })
                .A(new myClass { name="key1", age=1 }, new myClass { name="value1", age=1 })
                .A(new myClass { name="key1", age=1 }, new myClass { name="value1", age=1 })
                .A(new myClass { name="key1", age=1 }, new myClass { name="value1", age=7 })
            ;
            Assert.IsTrue(tt[new myClass { name="key1", age=1 }] == new myClass { name="value1", age=7 });
            Assert.IsTrue(tt[new myClass { name="key1", age=1 }] == "value1, 7");
            Assert.IsTrue(tt.ToDictionary().Count == 1);
            tt.ToDictionary().Add(new myClass { name="key2", age=2 }, new myClass { name="key2", age=2 });
            Assert.IsTrue(tt[new myClass { name="key2", age=2 }] == "key2, 2");

            var ro = new ReadonlySimpleTranslationTable<myClass>(tt);
            ro.ToDictionary().Add(new myClass { name="key3", age=3 }, new myClass { name="value3", age=3 });
            Assert.IsTrue(ro[new myClass { name="key3", age=3 }] == "key3, 3");

        }

        [TestMethod]
        public void complexType() {
            var tt = new SimpleTranslationTable<myClass>();
            var ro = new ReadonlySimpleTranslationTable<myClass>(tt);
            tt.Add(
                new myClass{ name="left", age=1 },
                new myClass{ name="right", age=2 }
            ).Add(
                new myClass(),
                new myClass{ name="noname", age=-1 }
            );
            var r = tt[new myClass { name="left", age=1 }];
            int i = new myClass { name="right", age=2 };
            Assert.IsTrue(r == "right, 2");
            Assert.IsTrue(r == i);

            var u = tt[new myClass()];
            i=new myClass { name="noname", age=-1 };
            Assert.IsTrue("noname, -1" == u);
            Assert.IsTrue(u == i);
        }

        [TestMethod]
        public void parallel() {
            Parallel.For(10, 100, (i) => threads(i));
        }
        public void threads(int iterations) {
            var tt = new SimpleTranslationTable<myClass>();
            var ro = new ReadonlySimpleTranslationTable<myClass>(tt);
            Parallel.For(1, iterations, (i) => {
                tt[new myClass { age=i }]=new myClass { age=i*2 };
            });

            Assert.IsTrue(tt.ToDictionary().Count() == iterations-1);

            Parallel.For(1, iterations, (i) => {
                Assert.IsTrue(ro[new myClass { age=i }] == new myClass { age=i*2 });
                Assert.IsFalse(ro[new myClass { age=i }] != new myClass { age=i*2 });
            });

        }
    }
}
