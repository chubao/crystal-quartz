namespace CrystalQuartz.Core
{
    using Quartz;

    public interface ISchedulerProvider
    {
        IScheduler Scheduler { get; }
    }
}