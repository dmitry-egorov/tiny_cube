using System;
using System.Text;

namespace Plugins.Lanski.Subjective
{
    internal struct TypeKey
    {
        public int key_index;
        public uint key_word;

        public TypeKey next()
        {
            if (key_word == 0)
                return new TypeKey { key_index = 0, key_word = 1u };
            
            var /* is the last key */ ilk = key_word == (1u << 31);

            var /* new key index */ ki = ilk ? key_index + 1 : key_index;
            var /*new key workd */ kw = ilk ? 1u : key_word << 1;
            
            return new TypeKey {key_index = ki, key_word = kw};
        }

        public override string ToString()
        {
            var sb = new StringBuilder();

            for (var i = 0; i < key_index; i++)
            {
                for (var j = 0; j < 32; j++)
                {
                    sb.Append('0');
                }
            }

            sb.Append(Convert.ToString(key_word, 2).PadLeft(32, '0'));
            
            return sb.ToString();
        }
    }
}