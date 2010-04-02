namespace CrystalQuartz.Web.FrontController.ResponseFilling
{
    using System.Web;

    public abstract class DefaultResponseFiller : IResponseFiller
    {
        public virtual string ContentType
        {
            get
            {
                return "text/html";
            }
        }

        public virtual int StatusCode
        {
            get
            {
                return 200;
            }
        }

        public void FillResponse(HttpResponseBase response, HttpContextBase context)
        {
            response.ContentType = ContentType;
            response.StatusCode = StatusCode;
            InternalFillResponse(response, context);
        }

        protected abstract void InternalFillResponse(HttpResponseBase response, HttpContextBase context);
    }
}