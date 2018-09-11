using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zettalith
{
    public delegate (string tag, Type type)[] ImportRequest();

    abstract class ContentObject
    {
        public static event ImportRequest ImportRequestStack;

        static ContentObject()
        {
            ImportRequest request = 

            ImportRequestStack += 
        }

        static (string tag, Type type)[] GetRequest()
        {
            return null;
        }
    }

    public interface IContentImport
    {
        (string tag, Type type)[] ImportRequests();
    }
}
