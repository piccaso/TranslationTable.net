using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Diagnostics;
using TranslationTable;
using System.Collections.Specialized;
using System.Collections;
using System.Linq;

namespace UnitTestProject1 {
    [TestClass]
    public class UnitTest1 {
        [TestMethod]
        public void TestMethod1() {
            var tt = new SimpleStringTranslationTable();
            tt["key"]="value";
            Assert.IsTrue(tt["key"]=="value");

            tt["key"]="xxx";
            Assert.IsTrue(tt["key"]=="xxx");

            tt["key"]=null;
            Assert.IsTrue(tt["key"]=="key");
        }


        [TestMethod, ExpectedException(typeof(ReadOnlyException))]
        public void TestMethod2() {
            var tt = new SimpleStringTranslationTable();
            tt["key"]="value";
            var ro = new ReadonlySimpleStringTranslationTable(tt);
            Assert.IsTrue("value"==ro["key"]);
            Assert.IsTrue("blah"==ro["blah"]);
            tt["key"]="xyz";
            Assert.IsTrue(ro["key"]=="xyz");
            ro["x"]="y"; /* throws */
            Assert.Fail();
        }

        [TestMethod]
        public void Test3() {
            var sd = new StringDictionary();
            var tt = new SimpleStringTranslationTable(sd);
            var ro = new ReadonlySimpleStringTranslationTable(tt);
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

            var rod = ro.ToStringDictionary();
            var rox = new SimpleStringTranslationTable(rod);
            rod.Add("blah", "blx");
            Assert.IsTrue(ro["blah", null]==null);
            Assert.IsTrue(rox["blah", null]=="blx");

        }
    }
}
