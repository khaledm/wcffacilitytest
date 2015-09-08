using Castle.Core.Logging;
using Castle.Facilities.Logging;
using Castle.Facilities.WcfIntegration;
using Castle.Facilities.WcfIntegration.Rest;
using Castle.MicroKernel.Registration;
using Castle.Services.Logging.Log4netIntegration;
using Castle.Windsor;
using log4net;
using log4net.Config;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.Web;
using System.Web.Security;
using System.Web.SessionState;

[assembly: XmlConfigurator(Watch = true)]
namespace WindsorWcfIntegration
{
    public class Global : System.Web.HttpApplication
    {
        private IWindsorContainer container;
        protected void Application_Start(object sender, EventArgs e)
        {
            log4net.Config.XmlConfigurator.Configure();
            container =
                 new WindsorContainer()
                 .AddFacility<LoggingFacility>(f => f.UseLog4Net())
                 .AddFacility<WcfFacility>(f=> f.CloseTimeout = TimeSpan.Zero)
                 .Register(
                     Component.For<ILogger>().ImplementedBy<Log4netLogger>(),
                Component.For<TestCallContextInitializer>(),
                Component.For<TestEndpointBehavior>(),
                Component.For<OperationProfilerManager>(),
                 Component.For<OperationProfilerEndpointBehavior>(),
                     Component.For<ICustomerService>()
                     .ImplementedBy<CustomerService>().AsWcfService(new RestServiceModel().Hosted()));
            //.Named("customerservice"));
            
        }

        protected void Session_Start(object sender, EventArgs e)
        {

        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {

        }

        protected void Application_AuthenticateRequest(object sender, EventArgs e)
        {

        }

        protected void Application_Error(object sender, EventArgs e)
        {

        }

        protected void Session_End(object sender, EventArgs e)
        {

        }

        protected void Application_End(object sender, EventArgs e)
        {
            if (container != null)
            {
                container.Dispose();
            }
        }
    }
}