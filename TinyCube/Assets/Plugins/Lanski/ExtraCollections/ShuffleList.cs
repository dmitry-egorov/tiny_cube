using System.Collections.Generic;

namespace Plugins.Lanski.ExtraCollections
{
    public class ShuffleList<T>
    {
        public T this[int index] => _list[index];
        public int Count => _list.Count;

        public Enumerator GetEnumerator() => new Enumerator(this);

        public int Add(T item)
        {
            var index = _list.Count;
            _list.Add(item);
            return index;
        }

        public void RemoveAt(int index)
        {
            var lastIndex = _list.Count - 1;
            if (index != lastIndex)
            {
                _list[index] = _list[lastIndex];
            }
            
            _list.RemoveAt(lastIndex);
        }

        public struct Enumerator
        {
            public Enumerator(ShuffleList<T> list)
            {
                _list = list;
                _index = -1;
            }

            public T Current => _list[_index];
            public bool MoveNext() => (++_index) < _list.Count;

            private int _index;
            private readonly ShuffleList<T> _list; 

        }

        private readonly List<T> _list = new List<T>(); 
    }
}