namespace CrystalQuartz.Core.Domain
{
    using System.Collections.Generic;

    public class SchedulerData
    {
        public string Name { get; set; }

        public string InstanceId { get; set; }

        public bool IsStarted
        {
            get
            {
                return Status == SchedulerStatus.Started;
            }
        }

        public bool CanStart
        {
            get
            {
                return Status == SchedulerStatus.Ready;
            }
        }

        public bool CanShutdown
        {
            get
            {
                return Status != SchedulerStatus.Shutdown;
            }
        }

        public IList<JobGroupData> JobGroups { get; set; }

        public IList<TriggerGroupData> TriggerGroups { get; set; }

        public SchedulerStatus Status { get; set; }


    }
}