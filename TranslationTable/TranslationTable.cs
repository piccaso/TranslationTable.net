using System.Collections;
using System.Collections.Specialized;

namespace TranslationTable
{

    public class ReadOnlyException : System.MemberAccessException { }

    public interface ISimpleStringTranslationTable{
        string this[string key] { get; set; }
        StringDictionary ToStringDictionary();
        ISimpleStringTranslationTable Add(string key, string value);
    }

    public class ReadonlySimpleStringTranslationTable : SimpleStringTranslationTable, ISimpleStringTranslationTable {
        public ReadonlySimpleStringTranslationTable(ISimpleStringTranslationTable initData = null) : base(initData) {
            seal();
        }

        public new StringDictionary ToStringDictionary() {
            var dict = base.ToStringDictionary();
            var rval = new StringDictionary();
            foreach(DictionaryEntry entry in base.ToStringDictionary()) {
                rval.Add((string)entry.Key, (string)entry.Value);
            }
            return rval;
        }
    }

    public class SimpleStringTranslationTable : ISimpleStringTranslationTable
    {
        private StringDictionary stringDict;
        private bool isSealed = false;
        private readonly object mutex = new object();
        private void init(StringDictionary initData) {
            lock(mutex) {
                stringDict = initData ?? new StringDictionary();
            }
        }

        protected internal void seal() {
            lock(mutex) { isSealed = true; }
        }

        public SimpleStringTranslationTable(StringDictionary initData) {
            init(initData);
        }
        public SimpleStringTranslationTable(ISimpleStringTranslationTable initData = null) {
            init(initData == null ? null : initData.ToStringDictionary());
        }

        public bool KeyAvailable(string key) {
            return null == this[key,null];
        }
        public ISimpleStringTranslationTable Add(string key, string value) {
            this[key]=value;
            return this;
        }


        public StringDictionary ToStringDictionary() {
            lock(mutex) { return stringDict; }
        }
        public string this[string key] {
            get { return this[key, key]; }
            set { this[key, null]=value; }
        }
        public string this[string key, string defaultValue]{
            get{
                lock(mutex) { return stringDict[key] ?? defaultValue; }
            }
            set { lock(mutex) {
                    if(isSealed) throw new ReadOnlyException();
                    if(stringDict[key] != null) stringDict.Remove(key);
                    if(value != null) { stringDict.Add(key, value); } 
            }}
        }
    }

}
