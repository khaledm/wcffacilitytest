using System;
using System.ServiceModel.Dispatcher;
using System.Xml.Linq;

namespace WindsorWcfIntegration
{
    internal class OperationProfilerParameterInspector : IParameterInspector
    {
        OperationProfilerManager manager;
        bool isOneWay;
        public OperationProfilerParameterInspector(OperationProfilerManager manager, bool isOneWay)
        {
            this.manager = manager;
            this.isOneWay = isOneWay;
        }

        public void AfterCall(string operationName, object[] outputs, object returnValue, object correlationState)
        {
            DateTime endCall = DateTime.Now;
            DateTime startCall = (DateTime)correlationState;
            TimeSpan operationDuration = endCall.Subtract(startCall);
            this.manager.AddCall(operationName, operationDuration.TotalMilliseconds);
        }

        public object BeforeCall(string operationName, object[] inputs)
        {
            XElement input = (XElement)inputs[0];
            return DateTime.Now;
        }
    }
}