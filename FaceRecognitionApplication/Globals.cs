using Microsoft.ProjectOxford.Face;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FaceRecognitionApplication
{
    /**
     Popis svih globalnih varijabli, poput access tokena i keyeva, te raznih id-ova potrebnih za odrađivanje FacialRec-a.
     **/
    public class Globals
    {
        public static FaceServiceClient faceServiceClient = new FaceServiceClient("secret...", "https://westeurope.api.cognitive.microsoft.com/face/v1.0/");
        public const string subscriptionKey = "secret...";
        public const string uriBase = "https://westeurope.api.cognitive.microsoft.com/face/v1.0/";
        public const string personGroupId = "secret...";
        public const string nazivGrupe = "secret...";
    }
}
