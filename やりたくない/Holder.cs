using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace やりたくない
{
    [Serializable]
    class Holder<T>
    {
        public Holder(T value)
        {
            held = value;
        }
        public T held;
    }
}
