namespace CrystalQuartz.Core.Domain
{
    public class Activity : NamedObject
    {
        public Activity(string name, ActivityStatus status) : base(name)
        {
            Status = status;
        }

        public ActivityStatus Status { get; private set; }

        public bool CanStart
        {
            get
            {
                return Status == ActivityStatus.Paused || Status == ActivityStatus.Mixed;
            }
        }

        public bool CanPause
        {
            get
            {
                return Status == ActivityStatus.Active || Status == ActivityStatus.Mixed;
            }
        }
    }
}