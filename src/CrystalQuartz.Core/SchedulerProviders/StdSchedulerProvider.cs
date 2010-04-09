namespace CrystalQuartz.Core.SchedulerProviders
{
    using System.Collections.Specialized;
    using Quartz;
    using Quartz.Impl;

    public class StdSchedulerProvider : ISchedulerProvider
    {
        protected IScheduler _scheduler;

        public void Init()
        {
            ISchedulerFactory schedulerFactory = new StdSchedulerFactory(GetSchedulerProperties());
            _scheduler = schedulerFactory.GetScheduler();
            InitScheduler(_scheduler);
        }

        protected virtual void InitScheduler(IScheduler scheduler)
        {
        }

        protected virtual NameValueCollection GetSchedulerProperties()
        {
            return new NameValueCollection();
        }

        public IScheduler Scheduler
        {
            get { return _scheduler; }
        }
    }
}