using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zettalith
{
    public delegate (string tag, Type type)[] ImportRequest();

    public abstract class ContentObject
    {
        public static event ImportRequest ImportRequestStack;

        static ContentObject()
        {
            ImportRequestStack += GetRequest;
        }

        static (string tag, Type type)[] GetRequest()
        {
            return null;
        }
    }
}
