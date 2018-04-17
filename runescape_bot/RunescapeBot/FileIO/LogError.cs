using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using RunescapeBot.BotPrograms;
using System.Drawing;

namespace RunescapeBot.FileIO
{
    public class LogError
    {
        /// <summary>
        /// Name of the settings file
        /// </summary>
        private const string FileName = "ErrorLog.xml";

        /// <summary>
        /// Path to the directory containing bot settings
        /// </summary>
        private static string DirectoryPath
        {
            get { return Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\Roboport"; }
        }

        /// <summary>
        /// Returns the directoryPath + fileName
        /// </summary>
        public static string FilePath
        {
            get { return DirectoryPath + "\\" + FileName; }
        }

        /// <summary>
        /// Logs basic error information. This will overwrite an existing error log.
        /// </summary>
        /// <param name="e">the exception to log an error for</param>
        public static void SimpleLog(Exception e)
        {
            File.WriteAllText(FilePath, e.ToString());
        }

        /// <summary>
        /// Saves a screenshot for debugging
        /// </summary>
        /// <param name="image">Image to save</param>
        /// <param name="name">File name. Should not include path or extension.</param>
        public static void ScreenShot(Color[,] image, string name)
        {
            string filePath = DirectoryPath + "\\" + name + ".png";
            DebugUtilities.SaveImageToFile(image, filePath);
        }
    }
}
