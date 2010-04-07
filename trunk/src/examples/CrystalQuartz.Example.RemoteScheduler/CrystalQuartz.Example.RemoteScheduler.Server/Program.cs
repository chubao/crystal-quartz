namespace CrystalQuartz.Example.RemoteScheduler.Server
{
    using System;
    using System.Collections.Specialized;
    using Quartz;
    using Quartz.Impl;
    using Quartz.Job;

    class Program
    {
        static void Main(string[] args)
        {
            var properties = new NameValueCollection();
            properties["quartz.scheduler.instanceName"] = "RemoteServerScheduler";

            // set thread pool info
            properties["quartz.threadPool.type"] = "Quartz.Simpl.SimpleThreadPool, Quartz";
            properties["quartz.threadPool.threadCount"] = "5";
            properties["quartz.threadPool.threadPriority"] = "Normal";

            // set remoting expoter
            properties["quartz.scheduler.exporter.type"] = "Quartz.Simpl.RemotingSchedulerExporter, Quartz";
            properties["quartz.scheduler.exporter.port"] = "555";
            properties["quartz.scheduler.exporter.bindName"] = "QuartzScheduler";
            properties["quartz.scheduler.exporter.channelType"] = "tcp";

            ISchedulerFactory sf = new StdSchedulerFactory(properties);
            IScheduler sched = sf.GetScheduler();

            // define the job and ask it to run
            JobDetail job = new JobDetail("remotelyAddedJob", "default", typeof(NoOpJob));
            JobDataMap map = new JobDataMap();
            map.Put("msg", "Your remotely added job has executed!");
            job.JobDataMap = map;
            CronTrigger trigger = new CronTrigger("remotelyAddedTrigger", "default", "remotelyAddedJob", "default", DateTime.UtcNow, null, "/5 * * ? * *");

            // schedule the job
            sched.ScheduleJob(job, trigger);

            sched.Start();
        }
    }
}
