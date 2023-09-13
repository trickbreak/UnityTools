
namespace Trickbreak.CoroutineScheduler
{
    internal abstract class ScheduleBase
    {
        public string Tag { get; }

        public int Priority { get; }



        public ScheduleBase(string tag, int priority)
        {
            Tag = tag;
            Priority = priority;
        }
    }
}
