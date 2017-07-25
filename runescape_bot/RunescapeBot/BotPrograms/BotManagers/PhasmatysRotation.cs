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
            double goldBarTime = ((PhasmatysRunParams)CurrentBotParams).GoldBars * GoldBracelets.SINGLE_CRAFTING_TIME;
            double steelBarTime = ((PhasmatysRunParams)CurrentBotParams).SteelBars * Cannonballs.SINGLE_SMITH_TIME;
            double bowTime = ((PhasmatysRunParams)CurrentBotParams).Bows * GoldBracelets.SINGLE_CRAFTING_TIME;
            List<double> weights = new List<double>() { goldBarTime, steelBarTime, bowTime };
            int botAction = Probability.ChooseFromWeights(weights);
            switch(botAction)
            {
                case -1:
                    CurrentBotParams.BotAction = BotRegistry.BotActions.DoNothing;
                    break;
                case 0:
                    CurrentBotParams.BotAction = BotRegistry.BotActions.GoldBracelets;
                    break;
                case 1:
                    CurrentBotParams.BotAction = BotRegistry.BotActions.Cannonballs;
                    break;
                case 2:
                    CurrentBotParams.BotAction = BotRegistry.BotActions.StringBows;
                    break;
                default:
                    return false;
            }
            return true;
        }
    }
}
