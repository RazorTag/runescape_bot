using RunescapeBot.BotPrograms;
using RunescapeBot.FileIO;
using System.Runtime.Serialization;

namespace RunescapeBot.FileIO
{
    [DataContract]
    public class SettingsData : ISerializable
    {
        public SettingsData() { }

        /// <summary>
        /// Serializes SettingsData
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            if (info == null) { throw new System.ArgumentNullException("info"); }

            info.AddValue(nameof(SoloBotSettings), SoloBotSettings);
            info.AddValue(nameof(PhasmatysSettings), PhasmatysSettings ?? new PhasmatysSettings());
            info.AddValue(nameof(SelectedTab), SelectedTab);
        }

        /// <summary>
        /// Saves the relevant run parameters to serialized objects
        /// </summary>
        /// <param name="runParams"></param>
        /// <param name="selectedTab"></param>
        public void Save(RunParams runParams)
        {
            SoloBotSettings = SoloBotSettings ?? new SoloBotSettings();
            SoloBotSettings.Save(runParams);
            PhasmatysSettings = PhasmatysSettings ?? new PhasmatysSettings();
            PhasmatysSettings.Save(runParams);
        }

        /// <summary>
        /// Loads the relevant run parameters from serialized objects
        /// </summary>
        /// <param name="runParams"></param>
        public void Load(ref RunParams runParams)
        {
            SoloBotSettings = SoloBotSettings ?? new SoloBotSettings();
            SoloBotSettings.Load(ref runParams);
            PhasmatysSettings = PhasmatysSettings ?? new PhasmatysSettings();
            PhasmatysSettings.Load(ref runParams);
        }

        /// <summary>
        /// Index of the most recently selected tab
        /// </summary>
        [DataMember]
        public int SelectedTab { get; set; }

        /// <summary>
        /// Settings for the standard solo bot
        /// </summary>
        [DataMember]
        public SoloBotSettings SoloBotSettings { get; set; }

        /// <summary>
        /// Settings for the phasmatys rotation bot
        /// </summary>
        [DataMember]
        public PhasmatysSettings PhasmatysSettings { get; set; }
    }
}
