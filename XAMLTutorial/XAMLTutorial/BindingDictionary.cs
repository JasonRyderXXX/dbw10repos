using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XAMLTutorial
{
    public class SafeBindingDictionary<tobject> : INotifyPropertyChanging, INotifyPropertyChanged,IDictionary<string, tobject>
    {
        private IDictionary<string, tobject> dict = new Dictionary<string, tobject>();
        public ICollection<string> Keys => dict.Keys;

        public ICollection<tobject> Values => dict.Values;

        public int Count => dict.Count;

        public bool IsReadOnly => dict.IsReadOnly;

        public tobject this[string key] { 
            get => dict.ContainsKey(key) ? dict[key] : default(tobject);
            set => SetProperty(key, value);
                }


        public event PropertyChangedEventHandler PropertyChanged = null;
        public event PropertyChangingEventHandler PropertyChanging = null;

        private void SetProperty(string key, tobject obj)
        {
            if (this.PropertyChanging != null)
                PropertyChanging(this, new PropertyChangingEventArgs(key));

            if (!dict.ContainsKey(key))
                dict.Add(key, obj);
            else
                dict[key] = obj;

            if (this.PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(key.ToString()));
        }
        public void Add(string key, tobject value) =>
            SetProperty(key, value);

        public bool ContainsKey(string key) =>
            dict.ContainsKey(key);

        public bool Remove(string key) =>
            dict.Remove(key);

        public bool TryGetValue(string key, out tobject value) =>
            dict.TryGetValue(key, out value);

        public void Add(KeyValuePair<string, tobject> item) =>
            SetProperty(item.Key, item.Value);


        public void Clear() =>
            dict.Clear();

        public bool Contains(KeyValuePair<string, tobject> item) =>
            dict.Contains(item);

        public void CopyTo(KeyValuePair<string, tobject>[] array, int arrayIndex) =>
            dict.CopyTo(array, arrayIndex);

        public bool Remove(KeyValuePair<string, tobject> item) =>
            dict.Remove(item);
        public IEnumerator<KeyValuePair<string, tobject>> GetEnumerator() =>
            dict.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() =>
            dict.GetEnumerator();
    }

    public class DictionaryObject<tobject>: DynamicObject,INotifyPropertyChanging,INotifyPropertyChanged
    {
        public DictionaryObject(IDictionary<string,object> obj)
        {
            Obj = obj;
        }
        
        private IDictionary<string, object> Obj { get; }
        
        public event PropertyChangingEventHandler PropertyChanging = null;
        public event PropertyChangedEventHandler PropertyChanged = null;

        public override bool TryGetMember(GetMemberBinder binder, out object result)=>
            Obj.TryGetValue(binder.Name, out result);
        public override bool TrySetMember(SetMemberBinder binder, object value)
        {
            if (PropertyChanging != null)
                PropertyChanging(this, new PropertyChangingEventArgs(binder.Name));
            Obj[binder.Name] = value;
            if (PropertyChanged != null)
                PropertyChanged(this,new PropertyChangedEventArgs(binder.Name));
            return true;
        }
        public override string ToString() =>
           JsonConvert.SerializeObject(Obj);
    }
}
