namespace CrystalQuartz.Core
{
    using System.Collections.Specialized;
    using Quartz;
    using Quartz.Impl;

    public class RemoteSchedulerProvider : ISchedulerProvider
    {
        private static IScheduler _scheduler;

        public string SchedulerHost { get; set;}

        public IScheduler Scheduler
        {
            get
            {
                if (_scheduler == null)
                {
                    var properties = new NameValueCollection();

                    properties["quartz.scheduler.proxy"] = "true";
                    properties["quartz.scheduler.proxy.address"] = SchedulerHost;
                    ISchedulerFactory sf = new StdSchedulerFactory(properties);

                    _scheduler = sf.GetScheduler();        
                }
                return _scheduler;
            }
        }
    }
}