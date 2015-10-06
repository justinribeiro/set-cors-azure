using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Auth;
using Microsoft.WindowsAzure.Storage.Shared.Protocol;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace setCors
{
    class Program
    {
        static void Main(string[] args)
        {
            
            StorageCredentials storageCredentials = new StorageCredentials(args[0], args[1]);
            CloudStorageAccount storageAccount = new CloudStorageAccount(storageCredentials, true);

            var blobClient = storageAccount.CreateCloudBlobClient();

            // Define our basics for quick op
            ServiceProperties blobServiceProperties = new ServiceProperties()
            {
                HourMetrics = null,
                MinuteMetrics = null,
                Logging = null,
            };

            // Define our CORS rules (wide open here)
            CorsRule corsRule = new CorsRule()
            {
                AllowedHeaders = new List<string>() { "*" },
                AllowedMethods = CorsHttpMethods.Get | CorsHttpMethods.Post | CorsHttpMethods.Head | CorsHttpMethods.Put,
                AllowedOrigins = new List<string>() { "*" },
                ExposedHeaders = new List<string>() { "*", "Accept-Ranges", "Content-Range"},
                
                MaxAgeInSeconds = 7200
            };

            // Set our rule
            blobServiceProperties.Cors.CorsRules.Add(corsRule);

            try 
            { 
                blobClient.SetServiceProperties(blobServiceProperties);
                Console.Out.WriteLine("CORS is rollin' for " + args[0]);
            }
            catch (Exception e)
            {
                Console.Out.WriteLine(e.ToString());
            }
        }
    }
}
