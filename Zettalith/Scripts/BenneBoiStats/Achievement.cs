using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zettalith
{
    [Serializable]
    class Achievement
    {
        public Achievements.VariableType Type { get; private set; }

        public string Description { get; private set; }

        public bool Complete { get; private set; }

        public object Value { get; set; }
        public object TargetValue { get; private set; }

        public Achievement(Achievements.VariableType type, string description, object targetValue)
        {
            Type = type;
            Description = description;
            TargetValue = targetValue;
        }

        public void Achieve()
        {
            Complete = true;
        }
    }
}
