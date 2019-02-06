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
        static string FullPath => path + fileName;

        static string path = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\Zettalith\UserData\";
        static string fileName = "UserData.zth";

        public static void Save(PersonalData data)
        {
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            File.WriteAllBytes(FullPath, Encrypt(Bytestreamer.ToBytes(data)));
        }

        public static PersonalData Load()
        {
            PersonalData temp = Bytestreamer.ToObject<PersonalData>(Encrypt(File.ReadAllBytes(FullPath)));

            return temp;

            //if (temp == null)
            //{

            //}
            //else
            //{
            //    return temp;
            //}
        }

        static byte[] Encrypt(byte[] data)
        {
            int length = data.Length;
            byte[] temp = new byte[length];
            #region T O P  S E C R E T
            byte key = 49;
            #endregion

            for (int i = 0; i < length; ++i)
            {
                temp[i] = (byte)(data[i] ^ key);
            }

            return temp;
        }
    }
}