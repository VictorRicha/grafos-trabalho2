using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GrafosLib.DataStructure
{
    public class DictionaryOfCollection<TKey, TElement> : IDictionaryOfCollection,
        IEnumerable<KeyValuePair<TKey, IEnumerable<TElement>>>
    {
        public DictionaryOfCollection(bool sortedKeys = false, bool ensureUniqueness = false)
        {
            _ensureUniqueness = ensureUniqueness;
            if (sortedKeys) _concreteDict = new SortedDictionary<TKey, IEnumerable<TElement>>();
            else _concreteDict = new Dictionary<TKey, IEnumerable<TElement>>();
        }

        private readonly IDictionary<TKey, IEnumerable<TElement>> _concreteDict;
        private readonly bool _ensureUniqueness;

        public IEnumerable<TKey> Keys
        {
            get { return _concreteDict.Keys; }
        }

        public IEnumerable<TElement> Values
        {
            get { return GetWhen(t => true); }
        }

        public void Add(TKey key, TElement element)
        {
            IEnumerable<TElement> list;
            if (_concreteDict.TryGetValue(key, out list))
            {
                ((ICollection<TElement>)list).Add(element);
            }
            else
            {
                ICollection<TElement> collection;
                if (_ensureUniqueness) collection = new HashSet<TElement>();
                else collection = new List<TElement>();

                collection.Add(element);
                _concreteDict.Add(key, collection);
            }
        }

        public bool ContainsKey(TKey topic)
        {
            return _concreteDict.ContainsKey(topic);
        }

        public IEnumerable<TElement> GetWhen(Func<TKey, bool> predicate)
        {
            var matchingElements = new List<TElement>();

            foreach (var key in _concreteDict.Keys.Where(predicate))
            {
                matchingElements.AddRange(_concreteDict[key]);
            }

            return matchingElements;
        }

        public IEnumerable<TElement> GetWhen(Func<TKey, bool> keyPredicate,
            Func<TElement, bool> valuePredicate)
        {
            return GetWhen(keyPredicate).Where(valuePredicate).ToList();
        }

        public bool ContainsElement(TKey key, TElement value)
        {
            return _concreteDict.ContainsKey(key) && _concreteDict[key].Contains(value);
        }

        public void AddRange(TKey key, IEnumerable<TElement> values)
        {
            foreach (var element in values) Add(key, element);
        }

        public IEnumerable<TElement> this[TKey c]
        {
            get
            {
                IEnumerable<TElement> values;
                TryGetValue(c, out values);
                return values;
            }
            set
            {
                Remove(c);
                AddRange(c, value);
            }
        }

        public int CountKeys
        {
            get { return _concreteDict.Count; }
        }

        public int CountElements
        {
            get { return Values.Count(); }
        }

        public IDictionary<TKey, IEnumerable<TElement>> ConcreteDictReference()
        {
            return _concreteDict;
        }

        public Dictionary<TKey, IEnumerable<TElement>> CloneAsDict()
        {
            return _concreteDict.Keys.ToDictionary(key => key, key => this[key]);
        }

        public bool TryGetValue(TKey key, out IEnumerable<TElement> value, bool copy = false)
        {
            IEnumerable<TElement> asList;
            var hasElement = _concreteDict.TryGetValue(key, out asList);
            value = asList == null
                ? Enumerable.Empty<TElement>()
                : copy ? new List<TElement>(asList) : asList;
            return hasElement;
        }

        public bool TryGetElement(TKey key, Func<TElement, bool> valuePredicate, out TElement value)
        {
            IEnumerable<TElement> asList;
            value = default(TElement);
            if (!TryGetValue(key, out asList) || !asList.Any(valuePredicate)) return false;

            value = asList.First(valuePredicate);

            return true;
        }

        public override string ToString()
        {
            var sb = new StringBuilder("{");
            foreach (var key in _concreteDict.Keys)
            {
                sb.AppendFormat("'{0}': ", key);
                sb.Append("{");
                foreach (var item in _concreteDict[key])
                {
                    sb.AppendFormat("{0},", item);
                }
                sb.Length -= 1;
                sb.AppendLine("}, ");
            }
            sb.Length -= 1;
            sb.Append("}");
            return sb.ToString();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public IEnumerator<KeyValuePair<TKey, IEnumerable<TElement>>> GetEnumerator()
        {
            return _concreteDict.GetEnumerator();
        }

        public bool Remove(TKey key)
        {
            return _concreteDict.Remove(key);
        }

        public bool Remove(TKey key, TElement value)
        {
            IEnumerable<TElement> list;
            var removed = _concreteDict.TryGetValue(key, out list) &&
                          ((ICollection<TElement>)list).Remove(value);
            if (removed && !list.Any()) Remove(key);
            return removed;
        }

        public bool RemoveWhen(Func<TKey, bool> predicate)
        {
            var a = _concreteDict.Keys.Where(predicate).ToList();
            return a.Aggregate(false, (current, key) => Remove(key) || current);
        }

        public IEnumerable<TKey> RemoveElementWhen(Func<TElement, bool> elementPredicate,
            Func<TKey, bool> keyPredicate = null)
        {
            var targetKeys = keyPredicate == null
                ? _concreteDict.Keys
                : _concreteDict.Keys.Where(keyPredicate);

            var involvedKeys = new HashSet<TKey>();
            foreach (var key in targetKeys.ToList())
            {
                var toRemove = _concreteDict[key].Where(elementPredicate).ToList();

                if (toRemove.Any()) involvedKeys.Add(key);

                foreach (var element in toRemove) Remove(key, element);
            }

            return involvedKeys;
        }

        public void Clear()
        {
            _concreteDict.Clear();
        }
    }

    //marking interface
    public interface IDictionaryOfCollection
    {
    }
}