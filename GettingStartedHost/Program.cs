using System;
using System.ServiceModel;
using System.ServiceModel.Description;
using GettingStartedLib;

namespace GettingStartedHost
{
    class Program
    {
        static void Main(string[] args)
        {
            // Step 1: Create a URI to serve as the base address.
            Uri baseAddressTcp = new Uri("net.tcp://localhost:8001/WCF");
            Uri baseAddressHttp = new Uri("http://localhost:8002/WCF");
            Uri baseAddressHttps = new Uri("https://localhost:8003/WCF");

            // Step 2: Create a ServiceHost instance.
            ServiceHost selfHost = new ServiceHost(typeof(CalculatorService), baseAddressTcp, baseAddressHttp, baseAddressHttps);

            try
            {
                // Step 3: Add a service endpoint.
                selfHost.AddServiceEndpoint(typeof(ICalculator), new NetTcpBinding(), "CalculatorService");
                selfHost.AddServiceEndpoint(typeof(ICalculator), new NetHttpBinding(), "CalculatorService");
                selfHost.AddServiceEndpoint(typeof(ICalculator), new NetHttpsBinding(), "CalculatorService");

                // Step 4: Enable metadata exchange.
                ServiceMetadataBehavior smbTcp = new ServiceMetadataBehavior();
                ServiceMetadataBehavior smbHttp = new ServiceMetadataBehavior();
                ServiceMetadataBehavior smbHttps = new ServiceMetadataBehavior();
                smbTcp.HttpGetEnabled = false;
                selfHost.Description.Behaviors.Add(smbTcp);
                smbHttp.HttpGetEnabled = true;
                selfHost.Description.Behaviors.Add(smbHttp);
                smbHttps.HttpsGetEnabled = true;
                selfHost.Description.Behaviors.Add(smbHttps);

                // Step 5: Start the service.
                selfHost.Open();
                Console.WriteLine("The service is ready.");

                // Close the ServiceHost to stop the service.
                Console.WriteLine("Press <Enter> to terminate the service.");
                Console.WriteLine();
                Console.ReadLine();
                selfHost.Close();
            }
            catch (CommunicationException ce)
            {
                Console.WriteLine("An exception occurred: {0}", ce.Message);
                selfHost.Abort();
            }
        }
    }
}
