using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zettalith
{
    class ParameterizedAction<T>
    {
        public Action<T> Function { get; set; }
        public T Value { get; set; }

        public ParameterizedAction(Action<T> func, T value)
        {
            Function = func;
            Value = value;
        }

        public void Activate() => Function.Invoke(Value);
    }
}
