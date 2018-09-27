
using System.IO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Azure.WebJobs.Host;
using Newtonsoft.Json;
using CreateRating.Model;
using System;
using MongoDB.Driver;
using System.Security.Authentication;
using RestSharp;
using System.Net;
using Model;

namespace CreateRating
{
    public static class Function1
    {
        [FunctionName("CreateRating")]
        public static IActionResult Run([HttpTrigger(AuthorizationLevel.Function, "post", Route = null)]HttpRequest req, TraceWriter log)
        {
            log.Info("C# HTTP trigger function processed a request.");

            string requestBody = new StreamReader(req.Body).ReadToEnd();

            UserRating userRating = JsonConvert.DeserializeObject<UserRating>(requestBody);
            if (!(userRating.Rating >= 0 && userRating.Rating <= 5))
            {
                return (ActionResult)new OkObjectResult($"Rating cannot be less than 0 or larger than 5");
            }

            userRating.Timestamp = DateTime.UtcNow;
            userRating.Id = Guid.NewGuid();

            var productResult = Get($"https://serverlessohproduct.trafficmanager.net/api/GetProduct?productid={userRating.ProductId}");
            log.Info("productResult fetched");

            var userResult = Get($"https://serverlessohuser.trafficmanager.net/api/GetUser?userid={userRating.UserId}");
            log.Info("userResult fetched");

            SaveToDb(userRating);
            log.Info("Saved to DB");

            return new OkObjectResult(userRating);
        }

        static string Get(string uri)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(uri);
            request.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;

            using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
            using (Stream stream = response.GetResponseStream())
            using (StreamReader reader = new StreamReader(stream))
            {
                return reader.ReadToEnd();
            }
        }

        static void SaveToDb(UserRating userRating)
        {
            string dbName = "hack2";
            string collectionName = "ratings";
            string host = "db-hack.documents.azure.com";

            //connect to the DB
            string connectionString = @"mongodb://db-hack:VcpCq8xtY7KFSJBYILXWKD4UStyzb3rsKIZeAnbm3bUzftbZqWBFM2u8CRHNEQdNRMc657GhD1DmGOjPIA4J4g==@db-hack.documents.azure.com:10255/?ssl=true&replicaSet=globaldb";
            MongoClientSettings settings = MongoClientSettings.FromUrl(new MongoUrl(connectionString));
            settings.SslSettings = new SslSettings() { EnabledSslProtocols = SslProtocols.Tls12 };
            var mongoClient = new MongoClient(settings);

            //get the DB and collection
            var database = mongoClient.GetDatabase(dbName);

            var ratingcollection = database.GetCollection<UserRating>(collectionName);
            ratingcollection.InsertOne(userRating);
        }
    }
}
