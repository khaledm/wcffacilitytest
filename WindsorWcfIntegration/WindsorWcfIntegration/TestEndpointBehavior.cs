using Castle.Core.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using System.ServiceModel.Dispatcher;
using System.Text;
using System.Threading.Tasks;

namespace WindsorWcfIntegration
{
    public class TestEndpointBehavior :  IEndpointBehavior
    {
        private TestCallContextInitializer _callContextInitializer;

        public TestEndpointBehavior(TestCallContextInitializer callContextInitializer)
        {
            _callContextInitializer = callContextInitializer;
        }

        public void AddBindingParameters(ServiceEndpoint endpoint, BindingParameterCollection bindingParameters)
        {
            //throw new NotImplementedException();
        }

        public void ApplyClientBehavior(ServiceEndpoint endpoint, ClientRuntime clientRuntime)
        {
            //throw new NotImplementedException();
        }

        public void ApplyDispatchBehavior(ServiceEndpoint endpoint, EndpointDispatcher endpointDispatcher)
        {
            foreach (var operation in endpointDispatcher.DispatchRuntime.Operations)
            {
                operation.CallContextInitializers.Add(_callContextInitializer);
            }
        }

        public void Validate(ServiceEndpoint endpoint)
        {
            //throw new NotImplementedException();
        }
    }

    public class TestCallContextInitializer : ICallContextInitializer
    {
        private readonly ILogger logger;
        public TestCallContextInitializer(ILogger logger)
        {
            this.logger = logger;
        }
        public object BeforeInvoke(InstanceContext instanceContext, IClientChannel channel, Message message)
        {
            logger.Warn("Before Invoke");
            
            return null;
        }
        public void AfterInvoke(object correlationState)
        {
            logger.Warn("After Invoke");
        }        
    }
}
