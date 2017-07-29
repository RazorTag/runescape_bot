using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace RunescapeBot.BotPrograms
{
    [Serializable]
    public class PhasmatysRunParams : RunParams
    {
        public PhasmatysRunParams() : base(false)
        {
            TaskComplete = new BotResponse(DoNothing);
        }

        /// <summary>
        /// Modifies the property for the active resource instead of modifying iterations
        /// </summary>
        public override int Iterations
        {
            get
            {
                switch (Resource)
                {
                    case ActiveProperty.GoldBars:
                        return GoldBars;
                    case ActiveProperty.SteelBars:
                        return SteelBars;
                    case ActiveProperty.Bows:
                        return Bows;
                    default:
                        return 0;
                }
            }
            set
            {
                switch (Resource)
                {
                    case ActiveProperty.GoldBars:
                        GoldBars = value;
                        break;
                    case ActiveProperty.SteelBars:
                        SteelBars = value;
                        break;
                    case ActiveProperty.Bows:
                        Bows = value;
                        break;
                }
            }
        }

        /// <summary>
        /// Number of gold bars remaining
        /// </summary>
        public int GoldBars { get;
            set; }

        /// <summary>
        /// Number of steel bars remaining
        /// </summary>
        public int SteelBars { get; set; }

        /// <summary>
        /// Number of unstrung bows remaining
        /// </summary>
        public int Bows { get; set; }

        /// <summary>
        /// List of resources that Phasmatys bots can use
        /// </summary>
        public enum ActiveProperty : int
        {
            GoldBars,
            SteelBars,
            Bows
        }

        /// <summary>
        /// The resource being used by the active bot
        /// </summary>
        [XmlIgnore]
        public ActiveProperty Resource { get; set; }
    }
}
