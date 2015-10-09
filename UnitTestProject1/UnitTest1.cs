using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Diagnostics;
using TranslationTable;
using System.Collections.Specialized;
using System.Collections;
using System.Linq;
using System.Collections.Generic;

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
    }
}
