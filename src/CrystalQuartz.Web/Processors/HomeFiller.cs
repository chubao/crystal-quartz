namespace CrystalQuartz.Web.Processors
{
    using System.Collections.Generic;
    using Core;
    using FrontController.ResponseFilling;
    using FrontController.ViewRendering;

    public class HomeFiller : ViewEngineResponseFiller
    {
        private readonly ISchedulerDataProvider _schedulerDataProvider;

        public HomeFiller(IViewEngine viewEngine, ISchedulerDataProvider schedulerDataProvider)
            : base(viewEngine)
        {
            _schedulerDataProvider = schedulerDataProvider;
        }

        protected override IDictionary<string, object> ViewData
        {
            get
            {
                return new Dictionary<string, object>
                             {
                                 {"data", _schedulerDataProvider.Data}
                             };
            }
        }

        protected override string ViewName
        {
            get { return "home"; }
        }
    }
}