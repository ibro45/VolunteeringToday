using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp5
{
    class ImageTools
    {
        public static Stream Base64ToImage(string base64String)
        {
            return new MemoryStream(Convert.FromBase64String(base64String));
        }
    }
}
