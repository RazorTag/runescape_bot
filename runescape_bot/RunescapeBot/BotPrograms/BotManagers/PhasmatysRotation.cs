using RunescapeBot.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RunescapeBot.BotPrograms
{
    public class PhasmatysRotation : SimpleRotation
    {
        public PhasmatysRotation(RunParams runParams, RunParamsList botList) : base(runParams, botList) { }

        /// <summary>
        /// Sets the next activity for the bot to perform
        /// </summary>
        /// <returns></returns>
        protected override bool SelectBotAction()
        {
            double goldBarTime = ((PhasmatysRunParams)CurrentRunParams).GoldBars * GoldBracelets.SINGLE_CRAFTING_TIME;
            double steelBarTime = ((PhasmatysRunParams)CurrentRunParams).SteelBars * Cannonballs.SINGLE_SMITH_TIME;
            double bowTime = ((PhasmatysRunParams)CurrentRunParams).Bows * GoldBracelets.SINGLE_CRAFTING_TIME;
            List<double> weights = new List<double>() { goldBarTime, steelBarTime, bowTime };
            int botAction = Probability.ChooseFromWeights(weights);
            switch(botAction)
            {
                case 0:
                    CurrentRunParams.BotAction = BotRegistry.BotActions.GoldBracelets;
                    break;
                case 1:
                    CurrentRunParams.BotAction = BotRegistry.BotActions.Cannonballs;
                    break;
                case 2:
                    CurrentRunParams.BotAction = BotRegistry.BotActions.StringBows;
                    break;
                default:
                    return false;
            }
            return true;
        }
    }
}
