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
    public class PhasmatysBot : ISerializable
    {
        public PhasmatysBot()
        {
            BotName = "New Bot";
        }

        /// <summary>
        /// Serializes settings for a single Phasmatys bot
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            if (info == null) { throw new System.ArgumentNullException("info"); }

            info.AddValue(nameof(BotName), BotName);
            info.AddValue(nameof(Login), Login);
            info.AddValue(nameof(Password), Password);
            info.AddValue(nameof(GoldBars), GoldBars);
            info.AddValue(nameof(SteelBars), SteelBars);
            info.AddValue(nameof(Bows), Bows);
        }

        [DataMember]
        public string BotName { get; set; }

        [DataMember]
        public string Login { get; set; }

        [DataMember]
        public string Password { get; set; }

        [DataMember]
        public int GoldBars { get; set; }

        [DataMember]
        public int SteelBars { get; set; }

        [DataMember]
        public int Bows { get; set; }

        /// <summary>
        /// Saves Phasmatys bot settings
        /// </summary>
        public void Save(PhasmatysParams phasmatysParams)
        {
            BotName = phasmatysParams.BotName;
            Login = phasmatysParams.Login;
            Password = phasmatysParams.Password;
            GoldBars = phasmatysParams.GoldBars;
            SteelBars = phasmatysParams.SteelBars;
            Bows = phasmatysParams.Bows;
        }

        /// <summary>
        /// Copies the loaded bot settings to RunParams
        /// </summary>
        /// <param name="phasmatysParams"></param>
        public void Load(ref PhasmatysParams phasmatysParams)
        {
            phasmatysParams.BotName = BotName;
            phasmatysParams.Login = Login;
            phasmatysParams.Password = Password;
            phasmatysParams.GoldBars = GoldBars;
            phasmatysParams.SteelBars = SteelBars;
            phasmatysParams.Bows = Bows;
        }
    }
}
