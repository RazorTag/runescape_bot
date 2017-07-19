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
    public class PhasmatysSettings : ISerializable
    {
        public PhasmatysSettings() { }

        /// <summary>
        /// Serializes PhasmatysSettings
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            if (info == null) { throw new System.ArgumentNullException("info"); }

            info.AddValue(nameof(Bots), Bots);
        }

        [DataMember]
        public int SelectedBot { get; set; }

        /// <summary>
        /// List of Phasmatys settings for individual bots
        /// </summary>
        [DataMember]
        public List<PhasmatysBot> Bots { get; set; }

        /// <summary>
        /// Saves Phasmatys manager bot settings
        /// </summary>
        public void Save(RunParams runParams)
        {
            for (int i = 0; i < Bots.Count; i++)
            {
                Bots[i].Save(runParams.PhasmatysParams[i]);
            }
        }

        /// <summary>
        /// Copies the loaded bot settings to RunParams
        /// </summary>
        /// <param name="runParams"></param>
        public void Load(ref RunParams runParams)
        {
            PhasmatysParams phasmatysParams;
            Bots = Bots ?? new List<PhasmatysBot>(Phasmatys.NUMBER_OF_BOTS);

            for (int i = 0; i < Bots.Count; i++)
            {
                phasmatysParams = runParams.PhasmatysParams[i];
                Bots[i].Load(ref phasmatysParams);
                runParams.PhasmatysParams[i] = phasmatysParams;
            }
        }
    }
}
