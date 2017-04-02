using System.Runtime.Serialization;

namespace RunescapeBot.FileIO
{
    [DataContract]
    public class SettingsData : ISerializable
    {
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
        public Start.BotActions BotAction { get; set; }

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
        /// Serializes SettingsData
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            if (info == null) { throw new System.ArgumentNullException("info"); }

            info.AddValue("Login", Login);
            info.AddValue("Password", Password);
            info.AddValue("BotAction", BotAction);
            info.AddValue("Iterations", Iterations);
            info.AddValue("ClientFilePath", ClientFilePath);
        }
    }
}
