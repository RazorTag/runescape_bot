using RunescapeBot.BotPrograms;
using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using System.Xml.Serialization;

namespace RunescapeBot.FileIO
{
    public class BotSettings
    {
        /// <summary>
        /// Name of the settings file
        /// </summary>
        private const string fileName = "BotSettings.xml";

        /// <summary>
        /// Path to the directory containing bot settings
        /// </summary>
        private string directoryPath;

        /// <summary>
        /// Returns the directoryPath + fileName
        /// </summary>
        private string filePath
        {
            get
            {
                return directoryPath + "\\" + fileName;
            }
        }

        /// <summary>
        /// Used to convert between StartParams and xml
        /// </summary>
        private XmlSerializer serializer;

        /// <summary>
        /// Loads the last used bot settings from disk
        /// </summary>
        public BotSettings()
        {
            directoryPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\Roboport";
            serializer = new XmlSerializer(typeof(RunParams), new Type[] { typeof(PhasmatysRunParams) });
        }

        /// <summary>
        /// Loads the last used settings for all bot programs
        /// </summary>
        /// <param name="botAction"></param>
        /// <returns>true if the load is successful</returns>
        public RunParams LoadSettings()
        {
            Stream stream = null;
            XmlDocument document = new XmlDocument();
            RunParams runParams;

            try
            {
                Directory.CreateDirectory(directoryPath);   //create the directory if it doesn't already exist
                stream = File.Open(filePath, FileMode.OpenOrCreate);
                runParams = (RunParams) serializer.Deserialize(stream);
            }
            catch
            {
                runParams = new RunParams();
            }
            finally
            {
                if (stream != null)
                {
                    stream.Close();
                }
            }

            return runParams;
        }

        /// <summary>
        /// Saves the last used settings for all bot programs to disk
        /// </summary>
        /// <returns>true if the save is successful</returns>
        public bool SaveSettings(RunParams runParams)
        {
            bool success = false;
            Stream stream = null;
            XmlDocument document = new XmlDocument();

            try
            {
                Directory.CreateDirectory(directoryPath);   //create the directory if it doesn't already exist
                stream = File.Open(filePath, FileMode.Create);
                serializer.Serialize(stream, runParams);
                success = true;
            }
            catch
            {
                //TODO
            }
            finally
            {
                stream.Close();
            }
            return success;
        }
    }
}
