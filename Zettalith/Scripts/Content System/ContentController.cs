﻿using System;
using System.Reflection;
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
    public class TestClass
    {
        [Import(typeof(string))]
        string[] Hello { get; }

        public TestClass()
        {
        }
    }

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

        public void Initialize(ContentManager content, bool importAll)
        {
            if (importAll)
            {
                ImportAll(content);
            }
        }

        public static void ImportAll(ContentManager content)
        {
            Type[] allTypes = Assembly.GetExecutingAssembly().GetTypes().Where(t => t.GetCustomAttribute<ImportContentAttribute>() != null) as Type[];

            foreach (Type currentClass in allTypes)
            {
                PropertyInfo[] properties = currentClass.GetProperties(BindingFlags.Static | BindingFlags.Public);

                foreach (PropertyInfo property in properties)
                {
                    if (property.GetCustomAttribute<ImportContentAttribute>() != null)
                    {
                        Type type = property.GetCustomAttribute<ImportAttribute>().type;

                        if (property.PropertyType == typeof(string[]))
                        {
                            string[] array = property.GetMethod.Invoke(null, null) as string[];

                            foreach (string item in array)
                            {
                                

                                singleton.contentDictionary.Add(item, content.Load<Type>(item));
                            }

                            continue;
                        }

                        throw new Exception("Tried to import a non-string array.");
                    }
                }
            }
        }

        //public T[] Import<T>(params string[] requests)
        //{

        //}

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
