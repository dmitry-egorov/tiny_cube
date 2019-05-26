using System.Collections.Generic;

namespace Plugins.Lanski.ExtraCollections
{
    public class ShuffleList<T>
    {
        public Enumerator GetEnumerator() => new Enumerator(_items);

        public Token Add(T item)
        {
            var index = _items.Count;
            
            _items.Add(item);
            
            var token = new Token { Index = index };
            _tokens.Add(token);
            
            return token;
        }

        public void Remove(Token token)
        {
            var lastIndex = _items.Count - 1;
            var index = token.Index;
            
            if (index != lastIndex)
            {
                var lastItem = _items[lastIndex];
                _items[index] = lastItem;
                var lastToken = _tokens[lastIndex];
                _tokens[index] = lastToken;
                lastToken.Index = index;
            }
            
            _tokens.RemoveAt(lastIndex);
            _items.RemoveAt(lastIndex);
        }

        public struct Enumerator
        {
            public Enumerator(List<T> list)
            {
                _list = list;
                _index = -1;
            }

            public T Current => _list[_index];
            public bool MoveNext() => ++_index < _list.Count;

            int _index;
            readonly List<T> _list; 
        }
        
        public class Token
        {
            internal int Index;
        }

        readonly List<T> _items = new List<T>(); 
        readonly List<Token> _tokens = new List<Token>(); 
    }
}