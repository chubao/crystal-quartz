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

        private static readonly ISchedulerProvider SchedulerProvider;

        static PagesHandler()
        {
            ViewEngine = new VelocityViewEngine();    
            ViewEngine.Init();
            SchedulerProvider = Configuration.ConfigUtils.SchedulerProvider;
            SchedulerDataProvider = new DefaultSchedulerDataProvider(SchedulerProvider);
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
                               new SingleParamRequestMatcher("command", "scheduler-start"),
                               new StartSchedulerFiller(SchedulerProvider)),
                           new DefaultRequestHandler(
                               new SingleParamRequestMatcher("command", "scheduler-stop"),
                               new StopSchedulerFiller(SchedulerProvider)),
                           new DefaultRequestHandler(
                               new CatchAllRequestMatcher(),
                               new HomeFiller(ViewEngine, SchedulerDataProvider))        
                       };
        }
    }
}