using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;

namespace TranslationTable
{

    public class ReadOnlyException : System.MemberAccessException { }

    public interface ISimpleTranslationTable<T>{
        T this[T key] { get; set; }
        Dictionary<T, T> ToDictionary();
        ISimpleTranslationTable<T> Add(T key, T value);
    }

    public class ReadonlySimpleTranslationTable : ReadonlySimpleTranslationTable<string>, ISimpleTranslationTable<string> {
        public ReadonlySimpleTranslationTable(ISimpleTranslationTable<string> initData = null) : base(initData) { }
    }
    public class ReadonlySimpleTranslationTable<T> : SimpleTranslationTable<T>, ISimpleTranslationTable<T> {
        public ReadonlySimpleTranslationTable(ISimpleTranslationTable<T> initData = null) : base(initData) {
            seal();
        }

        public new Dictionary<T, T> ToDictionary() {
            return new Dictionary<T, T>(base.ToDictionary());
        }
    }

    public class SimpleTranslationTable : SimpleTranslationTable<string>, ISimpleTranslationTable<string> { }
    public class SimpleTranslationTable<T> : ISimpleTranslationTable<T>
    {
        private Dictionary<T, T> dict;
        private bool isSealed = false;
        private readonly object mutex = new object();
        private void init(Dictionary<T, T> initData) {
            lock(mutex) {
                dict = initData ?? new Dictionary<T, T>();
            }
        }

        protected internal void seal() {
            lock(mutex) { isSealed = true; }
        }

        public SimpleTranslationTable(Dictionary<T, T> initData) {
            init(initData);
        }
        public SimpleTranslationTable(ISimpleTranslationTable<T> initData = null) {
            init(initData == null ? null : initData.ToDictionary());
        }

        public bool ContainsKey(T key) {
            lock(mutex) {
                return dict.ContainsKey(key);
            }
        }
        public ISimpleTranslationTable<T> Add(T key, T value) {
            this[key]=value;
            return this;
        }

        public Dictionary<T, T> ToDictionary() {
            lock(mutex) { return dict; }
        }
        public T this[T key] {
            get { return this[key, key]; }
            set { this[key, key]=value; }
        }
        public T this[T key, T defaultValue]{
            get{
                lock(mutex) {
                    return dict.ContainsKey(key) ? dict[key] : defaultValue;
                }
            }
            set { lock(mutex) {
                    if(isSealed) throw new ReadOnlyException();
                    if(dict.ContainsKey(key)) dict.Remove(key);
                    if(value != null) { dict.Add(key, value); } 
            }}
        }
    }

}
