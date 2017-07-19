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
        /// Bot settings data
        /// </summary>
        public SettingsData SettingsData;

        /// <summary>
        /// Loads the last used bot settings from disk
        /// </summary>
        public BotSettings()
        {
            directoryPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\Roboport";
            serializer = new XmlSerializer(typeof(SettingsData));
        }

        /// <summary>
        /// Loads the last used settings for all bot programs
        /// </summary>
        /// <param name="botAction"></param>
        /// <returns>true if the load is successful</returns>
        public bool LoadSettings()
        {
            Stream stream = null;
            XmlDocument document = new XmlDocument();

            try
            {
                Directory.CreateDirectory(directoryPath);   //create the directory if it doesn't already exist
                stream = File.Open(filePath, FileMode.Open);
                SettingsData = (SettingsData) serializer.Deserialize(stream);
            }
            catch
            {
                SettingsData = new SettingsData();
                return false;
            }
            finally
            {
                if (stream != null)
                {
                    stream.Close();
                }
            }

            return true;
        }

        /// <summary>
        /// Saves the last used settings for all bot programs to disk
        /// </summary>
        /// <returns>true if the save is successful</returns>
        public bool SaveSettings()
        {
            Stream stream = null;
            XmlDocument document = new XmlDocument();

            try
            {
                Directory.CreateDirectory(directoryPath);   //create the directory if it doesn't already exist
                stream = File.Open(filePath, FileMode.Create);
                serializer.Serialize(stream, SettingsData);
            }
            finally
            {
                stream.Close();
            }

            return true;
        }

        /// <summary>
        /// Updates the settings for a single bot program
        /// Saves to disk after updating
        /// </summary>
        /// <param name="runParams"></param>
        public void SaveRunParams(RunParams runParams)
        {
            SettingsData.Save(runParams);
        }

        /// <summary>
        /// Loads the settings data into RunParams
        /// </summary>
        /// <param name="runParams">runParams to store the settings</param>
        public void LoadRunParams(ref RunParams runParams)
        {
            SettingsData.Load(ref runParams);
        }
    }
}
