using System;
using System.Collections;

namespace Trickbreak.CoroutineScheduler
{
    internal class ScheduleCoroutine : ScheduleBase
    {
        private Func<IEnumerator> work;



        public ScheduleCoroutine(string tag, int priority, Func<IEnumerator> work) : base(tag, priority)
        {
            this.work = work;
        }

        public IEnumerator Invoke()
        {
            if (work == null)
            {
                yield break;
            }
                
            yield return work.Invoke();
        }
    }
}
