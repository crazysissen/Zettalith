using System;
using System.Reflection;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;

namespace Zettalith
{
    [ImportContent]
    public class TestClass
    {
        public static string[] Hello => new string[] { "authoritah", "Alve_Gud_2" }; 

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
            singleton.contentDictionary = new Dictionary<string, object>();

            (string name, string path)[] allFileNames = AllFileNames(AppDomain.CurrentDomain.BaseDirectory + "\\" + content.RootDirectory, "", "");

            foreach ((string name, string path) item in allFileNames)
            {
                singleton.contentDictionary.Add(item.name, content.Load<object>(item.path));
            }

            //Type[] allTypes = Assembly.GetExecutingAssembly().GetTypes();

            //foreach (Type currentClass in allTypes)
            //{
            //    PropertyInfo[] properties = currentClass.GetProperties(BindingFlags.Static | BindingFlags.Public);

            //    foreach (PropertyInfo property in properties)
            //    {
            //        if (property.GetCustomAttribute<ImportAttribute>() != null)
            //        {
            //            Type type = property.GetCustomAttribute<ImportAttribute>().type;

            //            if (property.PropertyType == typeof(string[]))
            //            {
            //                string[] array = property.GetMethod.Invoke(null, null) as string[];

            //                foreach (string item in array)
            //                {
            //                    singleton.contentDictionary.Add(item, Convert.ChangeType(content.Load<object>(item), type));
            //                }

            //                continue;
            //            }

            //            throw new Exception("Tried to import a non-string array.");
            //        }
            //    }
            //}
        }

        public static (string name, string path)[] AllFileNames(string basePath, string additionalPath, string appendableAdditionalPath)
        {
            List<(string name, string path)> allFiles = new List<(string name, string path)>();

            DirectoryInfo directory = new DirectoryInfo(basePath + "\\" + additionalPath);
            DirectoryInfo[] directories = directory.GetDirectories();
            FileInfo[] files = directory.GetFiles();

            foreach (FileInfo file in files)
            {
                allFiles.Add((Path.GetFileNameWithoutExtension(file.FullName), appendableAdditionalPath + Path.GetFileNameWithoutExtension(file.FullName)));
            }

            foreach (DirectoryInfo dir in directories)
            {
                allFiles.AddRange(AllFileNames(basePath, dir.Name + "\\", appendableAdditionalPath + dir.Name + "/"));
            }

            return allFiles.ToArray();
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
