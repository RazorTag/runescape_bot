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
            serializer = new XmlSerializer(typeof(RunParams), new Type[] { typeof(RotationRunParams), typeof(PhasmatysRunParams) });
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
                    stream.Close();
            }

            return runParams;
        }

        /// <summary>
        /// Saves the last used settings for all bot programs to disk
        /// </summary>
        /// <param name="runParams">Settings to save</param>
        /// <param name="failSafe">Set to true to save a backup copy of the existing settings before attempting to save.</param>
        /// <returns>true if the save is successful</returns>
        public bool SaveSettings(RunParams runParams)
        {
            bool success = false;
            Stream stream = null;
            XmlDocument document = new XmlDocument();
            SanitizeRunParams(runParams);

            try
            {
                Directory.CreateDirectory(directoryPath);   //create the directory if it doesn't already exist
                stream = File.Open(filePath, FileMode.OpenOrCreate);
                serializer.Serialize(stream, runParams);
                success = true;
            }
            catch { }
            finally
            {
                if (stream != null)
                    stream.Close();
            }
            return success;
        }

        /// <summary>
        /// Removes data that should not be serialized
        /// </summary>
        /// <param name="runParams"></param>
        private void SanitizeRunParams(RunParams runParams)
        {
            if (runParams != null)
            {
                if (runParams.RotationParams != null)
                {
                    for (int i = 0; i < runParams.RotationParams.Count; i++)
                    {
                        runParams.RotationParams.ParamsList[i].RotationParams = null;
                        runParams.RotationParams.ParamsList[i].PhasmatysParams = null;
                    }
                }

                if (runParams.PhasmatysParams != null)
                {
                    for (int i = 0; i < runParams.PhasmatysParams.Count; i++)
                    {
                        runParams.PhasmatysParams.ParamsList[i].RotationParams = null;
                        runParams.PhasmatysParams.ParamsList[i].PhasmatysParams = null;
                    }
                }
            }
        }
    }
}
