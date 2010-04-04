namespace CrystalQuartz.Core
{
    using System.Collections.Generic;
    using Domain;
    using Quartz;

    public class DefaultSchedulerDataProvider : ISchedulerDataProvider
    {
        private readonly ISchedulerProvider _schedulerProvider;

        public DefaultSchedulerDataProvider(ISchedulerProvider schedulerProvider)
        {
            _schedulerProvider = schedulerProvider;
        }

        public SchedulerData Data
        {
            get
            {
                var scheduler = _schedulerProvider.Scheduler;
                return new SchedulerData
                           {
                               Name = scheduler.SchedulerName,
                               InstanceId = scheduler.SchedulerInstanceId,
                               JobGroups = GetJobGroups(scheduler),
                               TriggerGroups = GetTriggerGroups(scheduler),
                               Status = GetSchedulerStatus(scheduler)
                           };
            }
        }

        public SchedulerStatus GetSchedulerStatus(IScheduler scheduler)
        {
            if (scheduler.IsShutdown)
            {
                return SchedulerStatus.Shutdown;
            }

            if (scheduler.JobGroupNames == null || scheduler.JobGroupNames.Length == 0)
            {
                return SchedulerStatus.Empty;
            }

            if (scheduler.IsStarted)
            {
                return SchedulerStatus.Started;
            }

            return SchedulerStatus.NotStarted;
        }
        
        private static ActivityStatus GetTriggerStatus(string triggerName, string triggerGroup, IScheduler scheduler)
        {
            var state = scheduler.GetTriggerState(triggerName, triggerGroup);
            switch (state)
            {
                case TriggerState.Paused:
                    return ActivityStatus.Paused;
                case TriggerState.Complete:
                    return ActivityStatus.Complete;
                default:
                    return ActivityStatus.Active;
            }
        }

        private static ActivityStatus GetTriggerStatus(Trigger trigger, IScheduler scheduler)
        {
            return GetTriggerStatus(trigger.Name, trigger.Group, scheduler);
        }

        private static IList<TriggerGroupData> GetTriggerGroups(IScheduler scheduler)
        {
            var result = new List<TriggerGroupData>();
            if (!scheduler.IsShutdown)
            {
                foreach (var groupName in scheduler.TriggerGroupNames)
                {
                    var data = new TriggerGroupData(groupName);
                    data.Init();
                    result.Add(data);
                }
            }

            return result;
        }

        private static IList<JobGroupData> GetJobGroups(IScheduler scheduler)
        {
            var result = new List<JobGroupData>();

            if (!scheduler.IsShutdown)
            {
                foreach (var groupName in scheduler.JobGroupNames)
                {
                    var groupData = new JobGroupData(
                        groupName,
                        GetJobs(scheduler, groupName));
                    groupData.Init();
                    result.Add(groupData);
                }
            }

            return result;
        }

        private static IList<JobData> GetJobs(IScheduler scheduler, string groupName)
        {
            var result = new List<JobData>();
            
            foreach (var jobName in scheduler.GetJobNames(groupName))
            {
                var jobData = new JobData(
                    jobName,
                    GetTriggers(scheduler, jobName));
                jobData.Init();
                result.Add(jobData);
            }

            return result;
        }

        private static IList<TriggerData> GetTriggers(IScheduler scheduler, string jobName)
        {
            var result = new List<TriggerData>();
            foreach (var groupName in scheduler.TriggerGroupNames)
            {
                foreach (var trigger in scheduler.GetTriggersOfJob(jobName, groupName))
                {
                    var data = new TriggerData(trigger.Name, GetTriggerStatus(trigger, scheduler))
                    {
                        StartDate = trigger.StartTimeUtc,
                        EndDate = trigger.EndTimeUtc,
                        NextFireDate = trigger.GetNextFireTimeUtc(),
                        PreviousFireDate = trigger.GetPreviousFireTimeUtc()
                    };
                    result.Add(data);
                }
            }

            return result;
        }
    }
}