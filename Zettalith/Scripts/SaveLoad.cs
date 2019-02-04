using System;
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

        //public static void Save(object data)
        //{
        //    File.WriteAllBytes(FullPath, Encrypt(Bytestreamer.ToBytes(data)));
        //}

        //public static object Load()
        //{

        //}

        //static byte[] Encrypt(byte[] data)
        //{
        //    int length = data.Length;
        //    byte[] temp = new byte[length];

        //    for (int i = 0; i < length; ++i)
        //    {
        //        temp[i] = (byte)(data[i] ^ 49);
        //    }

        //    return temp;
        //}

        //static byte[] Decrypt(byte[] data)
        //{

        //}
    }
}
