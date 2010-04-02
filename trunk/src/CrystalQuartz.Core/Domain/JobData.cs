namespace CrystalQuartz.Core.Domain
{
    using System.Collections.Generic;

    public class JobData : Activity
    {
        public JobData(string name, IList<TriggerData> triggers, ActivityStatus status): base(name, status)
        {
            Triggers = triggers;
        }

        public IList<TriggerData> Triggers { get; private set; }

        public bool HaveTriggers
        {
            get
            {
                return Triggers != null && Triggers.Count > 0;
            }
        }
    }
}