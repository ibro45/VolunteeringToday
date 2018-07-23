using Mono.Web;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using FaceRecognitionApplication;

namespace ConsoleApp5
{
    class RequestTraining
    {
        // Odvojena klasa za testiranje direktnih zahtjeva prema API-ju bez korištenja gotovih biblioteka
        // Omogućava bolje praćenje stanja modela te procesa treninga
        public static async void makeTrainRequest()
        {
            var client = new HttpClient();
            var queryString = HttpUtility.ParseQueryString(string.Empty);
            client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", Globals.subscriptionKey);
            var uri = "https://westeurope.api.cognitive.microsoft.com/face/v1.0/persongroups/" + Globals.personGroupId + "/train";
            HttpResponseMessage response;
            byte[] byteData = Encoding.UTF8.GetBytes("{body}");

            using (var content = new ByteArrayContent(byteData))
            {
                content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                response = await client.PostAsync(uri, content);
            }
            Console.WriteLine(response.IsSuccessStatusCode);
        }
    }
}
