using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Threading;

namespace OCRApplication
{
    /**
     Aplikacija za očitavanje teksta sa slike
     **/

    class Program
    {
        
        static string applicationPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
        
        // ucitavanje putanja do slika (plakata) koji se skeniraju
        static IEnumerable<string> ucitajSlike()
        {
            
            return Directory.EnumerateFiles(applicationPath + "\\lib\\", "*.*", SearchOption.AllDirectories)
            .Where(s => s.EndsWith(".jpg") || s.EndsWith(".png"));
        }
        // ucitavanje putanje za datoteku u koju se sprema očitani tekst sa slike
        static IEnumerable<string> ucitajRezultate()
        {
            return Directory.EnumerateFiles(applicationPath + "\\lib\\", "*.*", SearchOption.AllDirectories)
            .Where(s => s.EndsWith(".txt"));
        }




        static void Main()
        {
            
            var popisSlika = ucitajSlike();
            foreach (var imageFilePath in popisSlika)
            {
                OCRTools.analizirajSliku(imageFilePath);
                Thread.Sleep(5000);
            }
            List <TwitterJSON> twitterJSON = ucitajJsonIzDatoteke(applicationPath + @"\lib\twitter.json");


            // pristup datoteci s očitanim tekstom, izvlace se datum, email i 
            // naslov iz teksta te se zajedno s cjelim tekstom formatira u JSON
            var popisRezultata = ucitajRezultate();
            foreach (var rezultat in popisRezultata)
            {
                
                string fileStream = File.ReadAllText(rezultat);
                string ucitaniDatum = ucitajDatum(fileStream);
                string ucitaniEmail = "";
                List<string> ucitaniEmailLista = ucitajEmail(fileStream);
                foreach (var email in ucitaniEmailLista)
                {
                    ucitaniEmail += email + " ";
                }
                string ucitaniNaslov = ucitajNaslov(fileStream);
                string description = "Prijava na email: " + ucitaniEmail + "\r\n" + fileStream;
                twitterJSON.ForEach(x =>
                {
                    if (x.ActionImage.Contains(Path.GetFileNameWithoutExtension(rezultat)))
                    {
                        x.Description = description;
                        x.DateBegin = ucitaniDatum;
                        x.ActionName = ucitaniNaslov;
                    }
                });
                

                Thread.Sleep(5000);
            }
            spremiJsonUDatoteku(twitterJSON, applicationPath + @"\lib\twitter.json");
            posaljiJsonNaBackend(twitterJSON, "http://volunteering.westeurope.cloudapp.azure.com/api/Actions");
            Console.WriteLine("Yea boiiiiii");
            Console.ReadLine();

        }
        private static List<TwitterJSON> ucitajJsonIzDatoteke(string putanjaDoDatoteke)
        {
            using (StreamReader file = File.OpenText(putanjaDoDatoteke))
            {
                JsonSerializer serializer = new JsonSerializer();
                return (List <TwitterJSON>)serializer.Deserialize(file, typeof(List<TwitterJSON>));
            }
        }

        private static void spremiJsonUDatoteku(List<TwitterJSON> json, string putanjaDoDatoteke)
        {
            using (StreamWriter file = File.CreateText(putanjaDoDatoteke))
            {
                JsonSerializer serializer = new JsonSerializer();
                serializer.Serialize(file, json);
            }
        }
        private static void posaljiJsonNaBackend(List<TwitterJSON> json, string url)
        {
            string serijaliziraniJson = JsonConvert.SerializeObject(json);
            var httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
            httpWebRequest.ContentType = "application/json";
            httpWebRequest.Method = "POST";

            using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
            {
                streamWriter.Write(serijaliziraniJson);
                streamWriter.Flush();
                streamWriter.Close();
            }
            var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
            using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
            {
                var result = streamReader.ReadToEnd();
                Console.WriteLine(result);
            }
            
        }

        private static string ucitajDatum(string rezultat)
        {
            var regex = new Regex(@"\b\d{2}.\d{2}.\d{4}\b");
            foreach (Match m in regex.Matches(rezultat))
            {
                DateTime dt;
                if (DateTime.TryParseExact(m.Value, "dd.MM.yyyy", null, DateTimeStyles.None, out dt))
                    return dt.ToString();
            }
            return "Nema datuma";
        }


        private static List<string> ucitajEmail(string rezultat)
        {
            Regex reg = new Regex(@"[A-Z0-9._%+-]+@[A-Z0-9.-]+\.[A-Z]{2,6}", RegexOptions.IgnoreCase);
            Match match;

            List<string> emailovi = new List<string>();
            for (match = reg.Match(rezultat); match.Success; match = match.NextMatch())
            {
                if (!(emailovi.Contains(match.Value)))
                    emailovi.Add(match.Value);
            }

            return emailovi;
        }

        private static string ucitajNaslov(string rezultat)
        {
            string output = "";
            foreach (string rijec in rezultat.Split())
            {
                if (is_upper(rijec))
                    output += rijec + ' ';
                else
                    break;
            }
            return output;

            
        }
        private static bool is_upper(string userString)
        {
            bool provjera = false;
            foreach (char c in userString)
            {
                if (char.IsUpper(c) || c == 'Č' || c == 'Ć' || c == 'Š' || c == 'Ž')
                    provjera = true;
                else
                    provjera = false;
            }
            if (provjera == true)
                return true;
            else
                return false;
        }
    }
}