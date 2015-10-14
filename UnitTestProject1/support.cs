using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace UnitTestProject1 {
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]    
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

    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    public class myClass : CompareMeByToString {
        public string name { get; set; }
        public int age { get; set; }
        public override string ToString() {
            return (name ?? "?") + ", " + age.ToString();
        }
    }
}

namespace VW {
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    public class VW {
        private static bool CheckEnv(params string[] EnvVars) {
            int found = 0;
            foreach(var EnvVar in EnvVars) {
                if(!string.IsNullOrWhiteSpace(System.Environment.GetEnvironmentVariable(EnvVar))) found++;
            }
            return EnvVars.Length == found;
        }
        private static bool IsAgent() {
            return CheckEnv("AGENT_ID", "TF_BUILD", "AGENT_NAME");
        }
        public bool isAgent { get { Assert.IsTrue(true); return _isAgent; } }
        private bool _isAgent { get; set; }
        public VW() {
            _isAgent= IsAgent();
        }
    }
}
