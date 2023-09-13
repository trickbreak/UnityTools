using System;

namespace Trickbreak.CoroutineScheduler
{
    internal class ScheduleAction : ScheduleBase
    {
        private Action work;



        public ScheduleAction(string tag, int priority, Action work) : base(tag, priority)
        {
            this.work = work;
        }

        public void Invoke()
        {
            work?.Invoke();
        }
    }
}
