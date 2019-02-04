﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zettalith.Pieces;
using System.IO;

namespace Zettalith
{
    public static class SaveLoad
    {
        static string FullPath => Path + fileName;

        static string Path = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\Zettalith\UserData\";
        static string fileName = "UserData.zth";

        public static void Save(object data)
        {
            File.WriteAllBytes(FullPath, Bytestreamer.ToBytes(data));
        }

        //public static object Load()
        //{
            
        //}
    }
}