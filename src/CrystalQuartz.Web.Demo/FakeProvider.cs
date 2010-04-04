namespace CrystalQuartz.Web.Demo
{
    using System;
    using Core;
    using Quartz;
    using Quartz.Impl;

    public class FakeProvider : ISchedulerProvider
    {
        private static IScheduler _scheduler;

        static FakeProvider()
        {
            // construct a scheduler factory
            ISchedulerFactory schedFact = new StdSchedulerFactory();

            // get a scheduler
            IScheduler sched = schedFact.GetScheduler();
            //sched.Start();

            // construct job info
            JobDetail jobDetail = new JobDetail("myJob", null, typeof(HelloJob));
            // fire every hour
            jobDetail.Durable = true;
            Trigger trigger = TriggerUtils.MakeMinutelyTrigger(1, 2);
            // start on the next even hour
            trigger.StartTimeUtc = DateTime.UtcNow;
            trigger.Name = "myTrigger";
            sched.ScheduleJob(jobDetail, trigger);
            
            // construct job info
            JobDetail jobDetail2 = new JobDetail("myJob2", null, typeof(HelloJob));
            // fire every hour
            Trigger trigger2 = TriggerUtils.MakeMinutelyTrigger(1, 2);
            // start on the next even hour
            trigger2.StartTimeUtc = DateTime.UtcNow;
            trigger2.Name = "myTrigger2";
            sched.ScheduleJob(jobDetail2, trigger2);
            
            Trigger trigger3 = TriggerUtils.MakeSecondlyTrigger(5, 5);
            // start on the next even hour
            trigger3.StartTimeUtc = DateTime.UtcNow;
            trigger3.Name = "myTrigger3";
            trigger3.JobName = "myJob2";
            sched.ScheduleJob(trigger3);

            // construct job info
            JobDetail jobDetail4 = new JobDetail("myJob4", null, typeof(HelloJob));
            jobDetail4.Group = "MyOwnGroup";
            jobDetail4.JobDataMap.Add("key1", "value1");
            jobDetail4.JobDataMap.Add("key2", "value2");
            jobDetail4.JobDataMap.Add("key3", 1l);
            jobDetail4.JobDataMap.Add("key4", 1d);
            // fire every hour
            Trigger trigger4 = TriggerUtils.MakeMinutelyTrigger(1, 1);
            // start on the next even hour
            trigger4.StartTimeUtc = DateTime.UtcNow;
            trigger4.Name = "myTrigger4";
            trigger4.Group = jobDetail4.Group;
            sched.ScheduleJob(jobDetail4, trigger4);

            sched.PauseJob("myJob4", "MyOwnGroup");
            sched.PauseTrigger("myTrigger3", "DEFAULT");
            //sched.PauseJobGroup("DEFAULT");
            _scheduler = sched;
        }

        public IScheduler Scheduler
        {
            get
            {
                return _scheduler;
            }
        }
    }
}