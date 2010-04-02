namespace CrystalQuartz.Core.Domain
{
    using System.Collections.Generic;

    public class JobGroupData : Activity
    {
        public JobGroupData(string name, IList<JobData> jobs, ActivityStatus status) : base(name, status)
        {
            Jobs = jobs;
        }

        public IList<JobData> Jobs { get; private set; }
    }
}