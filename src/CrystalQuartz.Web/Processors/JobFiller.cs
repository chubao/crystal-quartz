namespace CrystalQuartz.Web.Processors
{
    using System.Collections.Generic;
    using Core;
    using FrontController.ResponseFilling;
    using FrontController.ViewRendering;

    public class JobFiller : ViewEngineResponseFiller
    {
        private readonly ISchedulerDataProvider _schedulerDataProvider;

        public JobFiller(IViewEngine viewEngine, ISchedulerDataProvider schedulerDataProvider)
            : base(viewEngine)
        {
            _schedulerDataProvider = schedulerDataProvider;
        }

        protected override IDictionary<string, object> ViewData
        {
            get
            {
                var jobName = Request.Params["job"];
                var jobGroup = Request.Params["group"];

                return new Dictionary<string, object>
                             {
                                 { "data", _schedulerDataProvider.Data },
                                 { "jobDetails", _schedulerDataProvider.GetJobDetailsData(jobName, jobGroup) }
                             };
            }
        }

        protected override string ViewName
        {
            get { return "job"; }
        }
    }
}