using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web;

namespace ShortURL.Helpers
{
    public class ServiceLocator
    {
        public static void ErrorLogger(string errorlog)
        {            
            string filePath = ConfigurationManager.AppSettings["ErrorLogPath"];
            string errorFileName = ConfigurationManager.AppSettings["ErrorFileName"];

            string fileName = Path.Combine(filePath, errorFileName + DateTime.Today.ToString("ddMMyyyy") + ".txt");
            using (StreamWriter file = new StreamWriter(fileName, true))
            {
                file.WriteLine(errorlog);
                file.Close();
            }
        }
    }
}