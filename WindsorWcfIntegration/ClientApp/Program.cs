using Castle.Core.Logging;
using Castle.Facilities.Logging;
using Castle.Facilities.WcfIntegration;
using Castle.MicroKernel.Registration;
using Castle.Services.Logging.Log4netIntegration;
using Castle.Windsor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using WindsorWcfIntegration;

[assembly: log4net.Config.XmlConfigurator(Watch = true)]
namespace ClientApp
{
    
    class Program
    {
        static void Main(string[] args)
        {
            var container = GetContainer();
            var reverser = container.Resolve<IDoSomethingWithCustomers>();
            reverser.DoIt();
            reverser.DoIt();
            reverser.DoSomeMore();

            //ServiceReference1.CustomerServiceClient client =
            //    new ServiceReference1.CustomerServiceClient(
            //        new WebHttpBinding(), 
            //        new EndpointAddress("http://localhost/WindsorWcfIntegration/Service1.svc"));
            //Console.WriteLine("{0}", client.GetData(new Random().Next()));

            Console.ReadLine();
        }

        private static IWindsorContainer GetContainer()
        {
            log4net.Config.XmlConfigurator.Configure();

            var container = new WindsorContainer()
                .AddFacility<LoggingFacility>(f => f.UseLog4Net())
                .AddFacility<WcfFacility>()
                .Register(
                 Component.For<ILogger>().ImplementedBy<Log4netLogger>(),
                Component.For<ICustomerService>().ImplementedBy<CustomerService>()
                .AsWcfClient(WcfEndpoint.BoundTo(new WebHttpBinding()).At("http://localhost/WindsorWcfIntegration/Service1.svc")));
                //.Named("customerservice"));
            container.Register(Component.For<IDoSomethingWithCustomers>().ImplementedBy<DoSomethingWithCustomers>());
            return container;
        }
    }

    public class DoSomethingWithCustomers : IDoSomethingWithCustomers
    {
        private readonly ICustomerService customerService;
        public DoSomethingWithCustomers(ICustomerService customerService)
        {
            this.customerService = customerService;
        }
        public void DoIt()
        {
            WriteClientDetails(customerService);
            WriteClientDetails(customerService);
        }
        private static void WriteClientDetails(ICustomerService customerService)
        {
            Console.WriteLine("{0}", customerService.GetData(new Random().Next()));
        }

        public void DoSomeMore()
        {
            var compositeType = customerService.GetDataUsingDataContract(new CompositeType { StringValue = "my name is ", BoolValue = true });
            Console.WriteLine("{0} {1}", compositeType.StringValue, compositeType.BoolValue );
    }
    }
}
