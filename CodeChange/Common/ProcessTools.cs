using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CodeChange.Common
{
    public class ProcessTools
    {
        public void pokreniProces(string proces, string argumenti)
        {
            System.Diagnostics.Process process = new System.Diagnostics.Process();
            System.Diagnostics.ProcessStartInfo startInfo = new System.Diagnostics.ProcessStartInfo();
            startInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
            startInfo.FileName = proces;
            startInfo.Arguments = argumenti;
            process.StartInfo = startInfo;
            process.Start();
        }
    }

}