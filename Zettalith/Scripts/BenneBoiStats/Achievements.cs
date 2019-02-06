using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zettalith
{
    static class Achievements
    {
        public enum VariableType { Bool, Double, Long }

        public static Dictionary<string, Achievement> locked = new Dictionary<string, Achievement>
        {
            { "Test", new Achievement(VariableType.Long, "Achieve five style points", 5) },
            //{ "Achievement name", new Achievement(VariableType.Double, "Achievement description", ) }
        };

        public static Dictionary<string, Achievement> unlocked = new Dictionary<string, Achievement>();

        public static void Check()
        {
            foreach (KeyValuePair<string, Achievement> achievement in locked)
            {
                if (achievement.Value.Type == VariableType.Bool)
                {
                    if ((bool)achievement.Value.Value == (bool)achievement.Value.TargetValue)
                    {
                        Complete(achievement);
                    }
                }
                else if (achievement.Value.Type == VariableType.Double)
                {
                    if ((double)achievement.Value.Value >= (double)achievement.Value.TargetValue)
                    {
                        Complete(achievement);
                    }
                }
                else if (achievement.Value.Type == VariableType.Long)
                {
                    if ((long)achievement.Value.Value >= (long)achievement.Value.Value)
                    {
                        Complete(achievement);
                    }
                }
            }
        }

        //public static void LoadAchievements()
        //{
        //    PersonalData data = SaveLoad.Load();
        //    locked = data.Locked;
        //    unlocked = data.Unlocked;
        //}

        static void Complete(KeyValuePair<string, Achievement> achievement)
        {
            locked.Remove(achievement.Key);
            unlocked.Add(achievement.Key, achievement.Value);
            achievement.Value.Achieve();
        }
    }
}
