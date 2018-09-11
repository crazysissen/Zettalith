using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;

namespace Zettalith
{
    public class ContentController
    {
        static ContentController singleton;

        Dictionary<string, object> contentDictionary;

        public ContentController(bool inheritContent)
        {
            if (singleton != null && inheritContent)
            {
                contentDictionary = singleton.contentDictionary;
            }

            singleton = this;
            contentDictionary = new Dictionary<string, object>();
        }

        public void Initialize(ContentManager content)
        {
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
