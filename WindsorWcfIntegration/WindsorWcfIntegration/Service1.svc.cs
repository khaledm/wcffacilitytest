using Castle.Core.Logging;
using System;
using System.Xml.Linq;

namespace WindsorWcfIntegration
{
    public class CustomerService : ICustomerService
    {
        private readonly ILogger _logger;

        public CustomerService(ILogger logger)
        {
            _logger = logger;
        }
        public string GetData(int value)
        {
            _logger.Info("in Get Data method now");
            return string.Format("You entered: {0}", value);
        }

        public CompositeType GetDataUsingDataContract(CompositeType composite)
        {
            _logger.Info("in GetDataUsingDataContract method now");
            if (composite == null)
            {
                throw new ArgumentNullException("composite");
            }
            if (composite.BoolValue)
            {
                composite.StringValue += "Suffix";
            }
            return composite;
        }

        public string PostData(XElement xml)
        {
            var xdoc = new XDocument(xml);
            return xdoc.ToString();
        }
    }
}
