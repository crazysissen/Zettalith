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

        public static string path = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\Zettalith\UserData\";
        public static string fileName = "UserData.zth";

        // TODO: Save userdata/Call this somewhere
        // Saves the current UserData to AppData/Roaming/Zettalith/UserData (PersonalData.UserData)
        public static void Save()
        {
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            File.WriteAllBytes(FullPath, Encrypt(Bytestreamer.ToBytes(PersonalData.UserData)));
        }

        // Sets current UserData to what is currently saved in save folder
        public static void Load()
        {
            PersonalData.UserData = Bytestreamer.ToObject<PersonalData>(Encrypt(File.ReadAllBytes(FullPath)));
        }

        // Saves default UserData if it's the first time the game is opened
        // If not, loads the player's currently saved data
        public static void Initialize()
        {
            if (!File.Exists(path + fileName))
            {
                PersonalData.UserData = PersonalData.Default;
                Save();
            }
            else
            {
                Load();
            }
        }

        // Encrypts and decrypts our savedata with a T O P  S E C R E T encryption keys
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