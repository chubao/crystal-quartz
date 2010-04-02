namespace CrystalQuartz.Web.FrontController.ResponseFilling
{
    using System.Collections.Generic;
    using System.Web;
    using ViewRendering;

    public abstract class ViewEngineResponseFiller : DefaultResponseFiller
    {
        private readonly IViewEngine _viewEngine;

        protected ViewEngineResponseFiller(IViewEngine viewEngine)
        {
            _viewEngine = viewEngine;
        }

        protected override void InternalFillResponse(HttpResponseBase response, HttpContextBase context)
        {
            _viewEngine.RenderView(ViewName, ViewData, response.OutputStream);
        }

        protected abstract IDictionary<string, object> ViewData { get; }

        protected abstract string ViewName { get; }
    }
}