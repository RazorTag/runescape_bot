using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RunescapeBot.BotPrograms
{
    public class PhasmatysRunParams : RunParams
    {
        public PhasmatysRunParams() : base(false) { }

        /// <summary>
        /// Number of gold bars remaining
        /// </summary>
        public int GoldBars { get; set; }

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
        public ActiveProperty Resource { get; set; }

        /// <summary>
        /// Update the active property
        /// </summary>
        /// <param name="iterations"></param>
        protected override void SetIterations(int iterations)
        {
            switch (Resource)
            {
                case ActiveProperty.GoldBars:
                    GoldBars = iterations;
                    break;
                case ActiveProperty.SteelBars:
                    SteelBars = iterations;
                    break;
                case ActiveProperty.Bows:
                    Bows = iterations;
                    break;
            }
        }
    }
}
