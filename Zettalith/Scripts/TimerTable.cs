using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zettalith
{
    class TimerTable
    {
        public float[] Times { get; private set; }
        public bool Complete { get; private set; }
        public float Current { get; set; }
        public float MaxTime { get; private set; }

        public float CurrentStepProgress { get; private set; }
        public float TotalProgress => Current / MaxTime;

        public TimerTable(float[] times, float startTime = 0.0f)
        {
            Current = startTime;
            Complete = startTime > times.Sum();

            SetTimes(times);
        }

        public int Update(float deltaTime)
        {
            Current += deltaTime;

            float accumulative = 0.0f;

            for (int i = 0; i < Times.Length; ++i)
            {
                if (Times[i] + accumulative > Current)
                {
                    CurrentStepProgress = (Current - accumulative) / Times[i];
                    return i;
                }

                accumulative += Times[i];
            }

            Complete = true;
            return Times.Length - 1;
        }

        public void SetTimes(float[] times)
        {
            Times = times;
            MaxTime = times.Sum();
        }
    }
}
