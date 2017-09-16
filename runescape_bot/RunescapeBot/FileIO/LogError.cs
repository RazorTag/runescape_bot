using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace RunescapeBot.FileIO
{
    public class LogError
    {
        /// <summary>
        /// Name of the settings file
        /// </summary>
        private const string fileName = "ErrorLog.xml";

        /// <summary>
        /// Path to the directory containing bot settings
        /// </summary>
        private static string directoryPath
        {
            get { return Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\Roboport"; }
        }

        /// <summary>
        /// Returns the directoryPath + fileName
        /// </summary>
        public static string FilePath
        {
            get { return directoryPath + "\\" + fileName; }
        }

        /// <summary>
        /// Logs basic error information. This will overwrite an existing error log.
        /// </summary>
        /// <param name="e">the exception to log an error for</param>
        public static void SimpleLog(Exception e)
        {
            File.WriteAllText(FilePath, e.ToString());
        }
    }
}
