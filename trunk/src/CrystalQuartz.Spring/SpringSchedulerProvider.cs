namespace CrystalQuartz.Spring
{
    using Core;
    using global::Spring.Context.Support;
    using Quartz;

    public class SpringSchedulerProvider : ISchedulerProvider
    {
        public IScheduler Scheduler
        {
            get
            {
                var applicationContext = ContextRegistry.GetContext();
                return (IScheduler) applicationContext.GetObject(SchedulerName);
            }
        }

        public string SchedulerName { get; set; }
    }
}