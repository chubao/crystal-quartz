namespace CrystalQuartz.Core
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
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

        private ActivityStatus GetTriggerGroupStatus(string groupName, IScheduler scheduler)
        {
            return scheduler.IsTriggerGroupPaused(groupName)
                       ? ActivityStatus.Paused
                       : ActivityStatus.Active;
        }

        private ActivityStatus GetJobGroupStatus(string groupName, IScheduler scheduler)
        {
            return scheduler.IsJobGroupPaused(groupName)
                       ? ActivityStatus.Paused
                       : ActivityStatus.Active;
        }

        private ActivityStatus GetJobStatus(string jobName, string groupName, IScheduler scheduler)
        {
            var triggers = scheduler.GetTriggersOfJob(jobName, groupName);
            if (triggers == null || triggers.Length == 0)
            {
                return ActivityStatus.Complete;
            }

            if (triggers.All(t => GetTriggerStatus(t, scheduler) == ActivityStatus.Paused))
            {
                return ActivityStatus.Paused;
            }

            if (triggers.All(t => GetTriggerStatus(t, scheduler) == ActivityStatus.Active))
            {
                return ActivityStatus.Active;
            }

            return ActivityStatus.Mixed;
        }

        private ActivityStatus GetTriggerStatus(string triggerName, string triggerGroup, IScheduler scheduler)
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

        private ActivityStatus GetTriggerStatus(Trigger trigger, IScheduler scheduler)
        {
            return GetTriggerStatus(trigger.Name, trigger.Group, scheduler);
        }

        private IList<TriggerGroupData> GetTriggerGroups(IScheduler scheduler)
        {
            var result = new List<TriggerGroupData>();
            if (!scheduler.IsShutdown)
            {
                foreach (var groupName in scheduler.TriggerGroupNames)
                {
                    result.Add(new TriggerGroupData(groupName, GetTriggerGroupStatus(groupName, scheduler)));
                }
            }

            return result;
        }

        private IList<JobGroupData> GetJobGroups(IScheduler scheduler)
        {
            var result = new List<JobGroupData>();

            if (!scheduler.IsShutdown)
            {
                foreach (var groupName in scheduler.JobGroupNames)
                {
                    result.Add(new JobGroupData(
                        groupName,
                        GetJobs(scheduler, groupName),
                        GetJobGroupStatus(groupName, scheduler)));
                }
            }

            return result;
        }

        private IList<JobData> GetJobs(IScheduler scheduler, string groupName)
        {
            var result = new List<JobData>();
            foreach (var jobName in scheduler.GetJobNames(groupName))
            {
                result.Add(new JobData(
                    jobName,
                    GetTriggers(scheduler, jobName),
                    GetJobStatus(jobName, groupName, scheduler)));
            }
            return result;
        }

        private IList<TriggerData> GetTriggers(IScheduler scheduler, string jobName)
        {
            var result = new List<TriggerData>();
            foreach (var groupName in scheduler.TriggerGroupNames)
            {
                foreach (var trigger in scheduler.GetTriggersOfJob(jobName, groupName))
                {
                    var data = new TriggerData(trigger.Name, GetTriggerStatus(trigger, scheduler));
                    data.StartDate = trigger.StartTimeUtc;
                    data.EndDate = trigger.EndTimeUtc;
                    data.NextFireDate = trigger.GetNextFireTimeUtc();
                    data.PreviousFireDate = trigger.GetPreviousFireTimeUtc();
                    result.Add(data);
                }
            }

            return result;
        }


    }
}