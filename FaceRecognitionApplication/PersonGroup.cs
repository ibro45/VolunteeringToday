using FaceRecognitionApplication;
using Microsoft.ProjectOxford.Face;
using Microsoft.ProjectOxford.Face.Contract;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp5
{
    class PersonGroup
    {
        /**
         * Klasa zadužena za stvaranje grupe osoba unutar koje se spremaju lica koja AI uči.
         * Svaku osobu je potrebno dodati unutar već predefinirane globalne grupe (do 10.000 različitih osoba max.).
         * Nakon dodavanja novog lica u grupu, model je potrebno ponovno istrenirati/dotrenirati.
         **/
        static Dictionary<string, CreatePersonResult> dictionary = new Dictionary<string, CreatePersonResult>();
        public static async void createPersonGroup()
        {
            try
            {
                await Globals.faceServiceClient.CreatePersonGroupAsync(Globals.personGroupId, Globals.nazivGrupe);
            }
            catch (Microsoft.ProjectOxford.Face.FaceAPIException e)
            {
                // Nothing needed :(
            }
        }

        // Dodavanje osobe preko GUID-a unutar grupe
        public static async void addPersonToGroup(string GUID)
        {
            var result = Task.Run(async () => { return await Globals.faceServiceClient.CreatePersonAsync(Globals.personGroupId, GUID); }).Result;
            dictionary.Add(GUID, result);
               
        }

        // Pridruživanje poznatog lica osobi sa proslijeđenim GUID-om radi prepoznavanja i kasnije mogućnosti korelacije
        public static async void addFaceToPerson(string base64, string GUID)
        {
            var personId = dictionary[GUID].PersonId;
            var result = Task.Run(async () => 
            {
                return await Globals.faceServiceClient.AddPersonFaceAsync(Globals.personGroupId, personId, ImageTools.Base64ToImage(base64));
            }).Result;
        }

        // Pozivanje metoda za dodavanje osobe te pripadajućeg lica u grupu
        public static void dodajOsobuUGrupu(string base64, string GUID)
        {
            addPersonToGroup(GUID);
            addFaceToPerson(base64, GUID);
        }

        // Pokretanje remote treniranja modela
        public static async void trainModel()
        {
            var result = Task.Run(async () => { return await Globals.faceServiceClient.GetPersonGroupTrainingStatusAsync(Globals.personGroupId); }).Result;
            Console.WriteLine(result.Status);

        }
        

}
}

