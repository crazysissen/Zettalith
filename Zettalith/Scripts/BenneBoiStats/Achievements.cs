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

        public static Dictionary<string, Achievement> DefaultLocked => new Dictionary<string, Achievement>
        {
            // Bool if false or true achievement, Double if decimal value achievment (ex float, double, decimal), Long if integer achievement (ex int, uint, long)
            { "Test", new Achievement(VariableType.Long, "Achieve five epic points", 5) },
            //{ "Achievement name", new Achievement(VariableType.Double, "Achievement description", ValueToBeReached) },
        };

        public static Dictionary<string, Achievement> DefaultUnlocked => new Dictionary<string, Achievement>();

        public static void Check()
        {
            foreach (KeyValuePair<string, Achievement> achievement in PersonalData.UserData.Locked)
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
                    if ((long)achievement.Value.Value >= (long)achievement.Value.TargetValue)
                    {
                        Complete(achievement);
                    }
                }
            }
        }

        // Run for completed achievment, moves from locked to unlocked dictionary
        static void Complete(KeyValuePair<string, Achievement> achievement)
        {
            PersonalData.UserData.Locked.Remove(achievement.Key);
            PersonalData.UserData.Unlocked.Add(achievement.Key, achievement.Value);
            //achievement.Value.Achieve();
        }
    }
}
