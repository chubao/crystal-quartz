namespace CrystalQuartz.Core.SchedulerProviders
{
    using System.Collections.Specialized;

    public class RemoteSchedulerProvider : StdSchedulerProvider
    {
        public string SchedulerHost { get; set;}

        protected override NameValueCollection GetSchedulerProperties()
        {
            var properties = base.GetSchedulerProperties();
            properties["quartz.scheduler.proxy"] = "true";
            properties["quartz.scheduler.proxy.address"] = SchedulerHost;
            return properties;
        }
    }
}