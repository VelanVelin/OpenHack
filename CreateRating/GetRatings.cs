
using System.IO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Azure.WebJobs.Host;
using Newtonsoft.Json;
using MongoDB.Driver;
using System.Security.Authentication;
using Model;

namespace CreateRating
{
    public static class GetRatings
    {
        [FunctionName("GetRatings")]
        public static IActionResult Run([HttpTrigger(AuthorizationLevel.Function, "get", Route = null)]HttpRequest req, TraceWriter log)
        {
            log.Info("C# HTTP trigger function processed a request.");

            string userId = req.Query["userId"];

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

            var result = ratingcollection.Find(x => x.UserId == new System.Guid(userId)).ToList();
            return (ActionResult)new OkObjectResult(result);
        }
    }
}
