using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using TranslationTable;

namespace UnitTestProject1 {
    [TestClass]
    public class UnitTest1 {
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

        public class CompareMeByToString {
            public static implicit operator string(CompareMeByToString t) {
                return t.ToString();
            }
            public static implicit operator int(CompareMeByToString t) {
                return ((string)t).Sum(o => (int)o);
            }
            public override int GetHashCode() { return this; }
            public override bool Equals(object obj) { return ((string)this) == ((CompareMeByToString)obj); }
            public static bool operator ==(CompareMeByToString left, CompareMeByToString right) { return (string)left == (string)right; }
            public static bool operator !=(CompareMeByToString left, CompareMeByToString right) { return (string)left != (string)right; }
        }

        public class myClass : CompareMeByToString {
            public string name { get; set; }
            public int age { get; set; }
            public override string ToString() {
                return (name ?? "?") + ", " + age.ToString();
            }
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
