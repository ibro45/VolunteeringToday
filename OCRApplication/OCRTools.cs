using System;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;

namespace OCRApplication
{
    public class OCRTools
    {
        // podaci potrebni za komunikaciju s Computer Vision API-jem
        const string subscriptionKey = "secret...";
        const string uriBase = " https://westeurope.api.cognitive.microsoft.com/vision/v1.0/ocr";
        public static object JSON { get; private set; }
        static string nazivDatoteke = "";

        // dobavlja analize specificirane slike koristeci Computer Vision API
        public static async void analizirajSliku(string imageFilePath)
        {
            nazivDatoteke = Path.GetFileNameWithoutExtension(imageFilePath);
            Console.WriteLine(nazivDatoteke);
            HttpClient client = new HttpClient();
            string uri = izgradiHTTPRequest(client);
            HttpResponseMessage response;
            byte[] byteData = dohvatiSlikuKaoByteArray(imageFilePath);
            response = await dohvatiResponseIEkstraktirajTekst(client, uri, byteData);
        }

        private static async System.Threading.Tasks.Task<HttpResponseMessage> dohvatiResponseIEkstraktirajTekst(HttpClient client, string uri, byte[] byteData)
        {
            HttpResponseMessage response;
            using (ByteArrayContent content = new ByteArrayContent(byteData))
            {
                content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
                response = await client.PostAsync(uri, content);
                string contentString = await response.Content.ReadAsStringAsync();
                zapisiJsonFile(contentString);
                ekstraktirajTekst();
            }

            return response;
        }

        private static string izgradiHTTPRequest(HttpClient client)
        {
            client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", subscriptionKey);
            string requestParameters = "language=unk&detectOrientation=true";
            string uri = uriBase + "?" + requestParameters;
            return uri;
        }

        private static void zapisiJsonFile(string contentString)
        {
            Console.WriteLine("\nResponse:\n");
            string json = JSONTools.JsonPrettyPrint(contentString);
            Console.WriteLine(json);
            string path = @"lib/" + nazivDatoteke + ".json";
            File.WriteAllText(path, json);
        }

        static byte[] dohvatiSlikuKaoByteArray(string imageFilePath)
        {
            FileStream fileStream = new FileStream(imageFilePath, FileMode.Open, FileAccess.Read);
            BinaryReader binaryReader = new BinaryReader(fileStream);
            return binaryReader.ReadBytes((int)fileStream.Length);
        }

        public static void ekstraktirajTekst()
        {
            ProcessTools.pokreniProces("python", @"lib/json_to_text.py lib/" + nazivDatoteke);
        }

        
    }
}

