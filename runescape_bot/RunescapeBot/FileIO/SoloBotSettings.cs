using RunescapeBot.BotPrograms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace RunescapeBot.FileIO
{
    [DataContract]
    public class SoloBotSettings : ISerializable
    {
        public SoloBotSettings() { }

        /// <summary>
        /// Serializes SoloBotSettings
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            if (info == null) { throw new System.ArgumentNullException("info"); }

            info.AddValue(nameof(Login), Login);
            info.AddValue(nameof(Password), Password);
            info.AddValue(nameof(BotAction), BotAction);
            info.AddValue(nameof(Iterations), Iterations);
            info.AddValue(nameof(ClientFilePath), ClientFilePath);
        }

        /// <summary>
        /// Last login name used
        /// </summary>
        [DataMember]
        public string Login { get; set; }

        /// <summary>
        /// Last password
        /// </summary>
        [DataMember]
        public string Password { get; set; }

        /// <summary>
        /// The most recently started bot program
        /// </summary>
        [DataMember]
        public BotRegistry.BotActions BotAction { get; set; }

        /// <summary>
        /// The number of times to run through a bot program's routine
        /// </summary>
        [DataMember]
        public int Iterations { get; set; }

        /// <summary>
        /// The file location of the RuneScape client to run
        /// </summary>
        [DataMember]
        public string ClientFilePath { get; set; }

        /// <summary>
        /// Saves general bot settings and solo bot settings
        /// </summary>
        /// <param name="generalSettings"></param>
        public void Save(RunParams generalSettings)
        {
            Login = generalSettings.Login;
            Password = generalSettings.Password;
            BotAction = generalSettings.BotAction;
            Iterations = generalSettings.Iterations;
            ClientFilePath = generalSettings.ClientFilePath;
        }

        /// <summary>
        /// Copies the loaded bot settings to RunParams
        /// </summary>
        /// <param name="generalSettings"></param>
        public void Load(ref RunParams generalSettings)
        {
            generalSettings.Login = Login;
            generalSettings.Password = Password;
            generalSettings.BotAction = BotAction;
            generalSettings.Iterations = Iterations;
            generalSettings.ClientFilePath = ClientFilePath;
        }
    }
}
