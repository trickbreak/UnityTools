using System;
using System.Collections;

namespace Trickbreak.CoroutineScheduler
{
    internal class Schedule
    {
        private Func<IEnumerator> work;
            
        public string Tag { get; }

        public int Priority { get; }
            

            
        public Schedule(string tag, int priority, Func<IEnumerator> work)
        {
            Tag = tag;
            Priority = priority;
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
