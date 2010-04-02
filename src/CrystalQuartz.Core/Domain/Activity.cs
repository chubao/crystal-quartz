namespace CrystalQuartz.Core.Domain
{
    public class Activity : NamedObject
    {
        public Activity(string name, ActivityStatus status) : base(name)
        {
            Status = status;
        }

        public ActivityStatus Status { get; private set; }
    }
}