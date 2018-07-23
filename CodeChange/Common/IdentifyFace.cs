using FaceRecognitionApplication;
using Microsoft.ProjectOxford.Face;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp5
{
    class IdentifyFace
    {
        public async static Task<List<string>> identifyFace(string base64)
        {
            Stream image = ImageTools.Base64ToImage(base64);
            var faces = await Globals.faceServiceClient.DetectAsync(image);
            var faceIds = faces.Select(face => face.FaceId).ToArray();

            var results = await Globals.faceServiceClient.IdentifyAsync(Globals.personGroupId, faceIds);
            List<string> guids = new List<string>();
            foreach (var identifyResult in results)
            {
                Console.WriteLine("Result of face: {0}", identifyResult.FaceId);
                if (identifyResult.Candidates.Length == 0)
                {
                    Console.WriteLine("No one identified");
                }
                else
                {
                    var candidateId = identifyResult.Candidates[0].PersonId;
                    var person = await Globals.faceServiceClient.GetPersonAsync(Globals.personGroupId, candidateId);
                    Console.WriteLine("Identified as {0}", person.Name);
                    guids.Add(person.Name);
                }
            }
            return guids;
        }
    }
}
