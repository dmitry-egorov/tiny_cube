using System;
using System.Linq;

namespace Plugins.Lanski.Subjective
{
    internal class TypeMask
    {
        public void add<T>() => add(TypeKeysStorage<T>.Key);
        public void add(Type t) => add(TypeKeysStorage.GetKey(t));
        
        public void add(TypeKey k)
        {
            var ki = k.key_index;
            var kw = k.key_word;

            if (_mask == null || _mask.Length < ki + 1)
            {
                var /* old mask */ om = _mask;
                _mask = new uint[ki + 1];
                om?.CopyTo(_mask, 0);
            }

            _mask[ki] ^= kw;
        }
        

        public void remove<T>() => remove(TypeKeysStorage<T>.Key);

        void remove(TypeKey key)
        {
            _mask[key.key_index] &= ~key.key_word;
        }

        public bool contains_all(TypeMask other)
        {
            var /* this mask  */ tm = _mask;
            var /* other mask */ om = other._mask;

            var tml = tm.Length;
            var oml = om.Length;
            
            var sl = Math.Min(tml, oml);

            var i = 0;
            for (; i < sl; i++)
            {
                var /* this word  */ tw = tm[i];
                var /* other word */ ow = om[i];
                if ((tw & ow) != ow)
                {
                    return false;
                }
            }

            if (oml > tml)
            {
                for (; i < oml; i++)
                {
                    if (om[i] != 0)
                        return false;
                }
            }

            return true;
        }
        
        public bool contains_any(TypeMask other)
        {
            var /* this mask  */ tm = _mask;
            var /* other mask */ om = other._mask;

            var tml = tm.Length;
            var oml = om.Length;
            
            var sl = Math.Min(tml, oml);

            var i = 0;
            for (; i < sl; i++)
            {
                var /* this word  */ tw = tm[i];
                var /* other word */ ow = om[i];
                if ((tw & ow) != 0)
                {
                    return true;
                }
            }

            return false;
        }

        public override string ToString() => 
            string.Join("", _mask.Select(i => Convert.ToString(i, 2).PadLeft(32, '0')).ToArray())
        ;

        uint[] _mask = {0};
    }
}