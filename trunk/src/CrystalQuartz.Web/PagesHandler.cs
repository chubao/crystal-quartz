namespace CrystalQuartz.Web
{
    using System.Collections.Generic;
    using Core;
    using FrontController;
    using FrontController.RequestMatching;
    using FrontController.ViewRendering;
    using Processors;

    public class PagesHandler : FrontControllerHandler
    {
        private static readonly IViewEngine ViewEngine;

        private static readonly ISchedulerDataProvider SchedulerDataProvider;

        static PagesHandler()
        {
            ViewEngine = new VelocityViewEngine();    
            ViewEngine.Init();
            SchedulerDataProvider = new DefaultSchedulerDataProvider(Configuration.ConfigUtils.SchedulerProvider);
        }

        public PagesHandler() : base(GetProcessors())
        {
        }

        private static IList<IRequestHandler> GetProcessors()
        {
            return new List<IRequestHandler>
                       {
                           new FileRequestProcessor(),
                           new DefaultRequestHandler(
                               new CatchAllRequestMatcher(),
                               new HomeFiller(ViewEngine, SchedulerDataProvider))        
                       };
        }
    }
}