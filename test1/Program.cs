using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace test1
{
    class Program
    {
        static void Main(string[] args)
        {
            // The code provided will print ‘Hello World’ to the console.
            // Press Ctrl+F5 (or go to Debug > Start Without Debugging) to run your app.
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            var client = new RestClient($"https://serverlessohproduct.trafficmanager.net/api/GetProduct?productid=75542e38-563f-436f-adeb-f426f1dabb5c");
            var request = new RestRequest(Method.GET);
            request.AddHeader("Postman-Token", "f201143c-1d96-4120-8079-3b2641756ba4");
            request.AddHeader("Cache-Control", "no-cache");
            IRestResponse response = client.Execute(request);

            Console.ReadKey();

            // Go to http://aka.ms/dotnet-get-started-console to continue learning how to build a console app! 
        }
    }
}
