namespace CrystalQuartz.Core.Domain
{
    using System.Collections.Generic;

    public class SchedulerData
    {
        public string Name { get; set; }

        public bool IsStarted { get; set; }

        public IList<JobGroupData> JobGroups { get; set; }

        public IList<TriggerGroupData> TriggerGroups { get; set; }

        public SchedulerStatus Status { get; set; }
    }
}