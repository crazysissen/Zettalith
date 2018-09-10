using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zettalith
{
    class ContentController
    {
        static ContentController singleton;

        Dictionary<string, object> contentDictionary;

        public ContentController()
        {
            singleton = this;
            contentDictionary = new Dictionary<string, object>();
        }

        public static T Get<T>(string tag)
        {
            try
            {
                if (singleton.contentDictionary[tag] is T)
                {
                    return (T)singleton.contentDictionary[tag];
                }

                return default(T);
            }
            catch
            {
                return default(T);
            }
        }
    }
}
