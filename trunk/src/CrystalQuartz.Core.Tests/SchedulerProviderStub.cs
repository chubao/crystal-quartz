namespace CrystalQuartz.Core.Tests
{
    using Quartz;

    public class SchedulerProviderStub : ISchedulerProvider
    {
        public SchedulerProviderStub(IScheduler scheduler)
        {
            Scheduler = scheduler;
        }

        public SchedulerProviderStub()
        {
        }

        public IScheduler Scheduler
        {
            get; set;
        }
    }
}