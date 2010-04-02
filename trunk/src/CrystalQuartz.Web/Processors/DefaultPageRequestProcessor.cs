namespace CrystalQuartz.Web.Processors
{
    using System;
    using System.IO;
    using System.Text;
    using System.Web;
    using Commons.Collections;
    using Core;
    using Core.Domain;
    using FrontController;
    using NVelocity;
    using NVelocity.App;

    public class DefaultPageRequestProcessor : IRequestHandler//MasterContentRequestProcessor
    {
        private readonly ISchedulerDataProvider _schedulerDataProvider;

        private const string SPAN_NONE = @"<span class='none'>[none]</span>";

        public DefaultPageRequestProcessor(ISchedulerDataProvider schedulerDataProvider)
        {
            _schedulerDataProvider = schedulerDataProvider;
        }

//        protected override string GetBodyContent(HttpContextBase context)
//        {
//            
//
//
////            var data = _schedulerDataProvider.Data;
//
//            var builder = new StringBuilder();
//            builder.AppendFormat(
//@"<div id='schedulerProperties' class='simplePanel'>
//    <h2>Scheduler properties</h2>
//    <div class='primary'>
//        <table>
//            <tr>
//                <th>Name:</th>
//                <td>{0}</td>
//            </tr>
//            <tr>
//                <th>Status:</th>
//                <td><span class='{1}'>{1}</span></td>
//            </tr>
//        </table>        
//    </div>
//    <div class='secondary'></div>
//</div>", data.Name, data.Status.ToString().ToLower());
//
//            if (data.JobGroups.Count == 0)
//            {
//                builder.Append("<div id='jobsContainer'>No jobs found</div>");
//            }
//            else
//            {
//                builder.Append("<div id='jobsContainer'>");
//                foreach (var group in data.JobGroups)
//                {
//                    builder.AppendFormat(
//@"<h2 class='groupHeader'>
//    {1} {0}
//</h2>", group.Name, GetActivityStatusSpan(group));
//                    foreach (var job in group.Jobs)
//                    {
//                        builder.Append("<div class='job'>");
//                        builder.AppendFormat(
//@"
//<div class='status'>
//    {3}
//</div>
//<h3>{1} &rarr; {0}</h3>
//<div class='secondary'>
//    <table>
//        <thead>
//            <tr>
//                <th>Name</th>
//                <th>Status</th>
//                <th>Start date</th>
//                <th>End date</th>
//                <th>Previous fire date</th>
//                <th>Next fire date</th>
//            </tr>
//        </thead>
//        <tbody>
//            {2}
//        </tbody>
//    </table>
//</div></div>
//",
//        job.Name,
//        group.Name,
//        GetTriggersTable(job),
//        GetActivityStatusSpan(job));
//                        builder.AppendFormat(
//@"");
//
//                    }
//                }
//
//            }
//            return builder.ToString();
//        }

        private string GetActivityStatusSpan(Activity group)
        {
            return string.Format(
                "<span class='{0}'>{1}</span>",
                group.Status.ToString().ToLower(),
                group.Status);
        }

        private string GetTriggersTable(JobData job)
        {
            var builder = new StringBuilder();
            if (job.HaveTriggers)
            {
                foreach (var trigger in job.Triggers)
                {

                    builder.AppendLine(string.Format(
    @"  
<tr>
        <td>{0}</td>
        <td>{1}</td>
        <td>{2}</td>
        <td>{3}</td>
        <td>{4}</td>
        <td>{5}</td>
    </tr>",
                                       trigger.Name,
                                       GetActivityStatusSpan(trigger),
                                       trigger.StartDate,
                                       trigger.EndDate == null ? SPAN_NONE : trigger.EndDate.ToString(),
                                       trigger.PreviousFireDate == null ? SPAN_NONE : trigger.PreviousFireDate.ToString(),
                                       trigger.NextFireDate == null ? SPAN_NONE : trigger.NextFireDate.ToString()
                                       ));
                }
            } 
            else
            {
                builder.Append("<tr><td class='none' colspan='6'>No active triggers</td></tr>");
            }
            return builder.ToString();
        }

        public bool HandleRequest(HttpContextBase context)
        {
            var data = _schedulerDataProvider.Data;

            VelocityEngine velocity = new VelocityEngine();

            ExtendedProperties props = new ExtendedProperties();
            
            props.AddProperty("resource.loader", "assembly");
            props.AddProperty("assembly.resource.loader.class",
                "NVelocity.Runtime.Resource.Loader.AssemblyResourceLoader, NVelocity");
            props.AddProperty("assembly.resource.loader.assembly", "CrystalQuartz.Web");
            velocity.Init(props);
            Template template = velocity.GetTemplate(@"CrystalQuartz.Web.Templates.home.vm");
            VelocityContext vcontext = new VelocityContext();
            vcontext.Put("data", data);
            using (var writer = new StreamWriter(context.Response.OutputStream))
            {
                template.Merge(vcontext, writer);
            }

            return true;
        }
    }
}