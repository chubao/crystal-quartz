namespace CrystalQuartz.Core.Domain
{
    using System.Collections.Generic;

    public class JobData : ActivityNode<TriggerData>
    {
        public JobData(string name, IList<TriggerData> triggers): base(name)
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

        protected override IList<TriggerData> ChildrenActivities
        {
            get { return Triggers; }
        }
    }
}