using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zettalith.Pieces;
using System.IO;

namespace Zettalith
{
    static class SaveLoad
    {
        static string FullPath => Path + fileName;

        static string Path = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\Zettalith\UserData\";
        static string fileName = "UserData.zth";

        public static void Save(PersonalData data)
        {
            if (!Directory.Exists(FullPath))
            {
                Directory.CreateDirectory(Path);
            }
            File.WriteAllBytes(FullPath, Encrypt(Bytestreamer.ToBytes(data)));
        }

        public static PersonalData Load()
        {
            return Bytestreamer.ToObject<PersonalData>(Encrypt(File.ReadAllBytes(FullPath)));
        }

        static byte[] Encrypt(byte[] data)
        {
            int length = data.Length;
            byte[] temp = new byte[length];
            byte key = 49;

            for (int i = 0; i < length; ++i)
            {
                temp[i] = (byte)(data[i] ^ key);
            }

            return temp;
        }
    }
}
