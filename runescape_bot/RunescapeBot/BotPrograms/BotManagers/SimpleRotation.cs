using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RunescapeBot.BotPrograms
{
    public class SimpleRotation : BotProgram
    {
        public const int NUMBER_OF_BOTS = 3;
        protected RunParamsList BotParams;
        private int CurrentBotIndex;
        public BotProgram CurrentBot;
        public RunParams CurrentBotParams { get { return BotParams[CurrentBotIndex]; } }

        public SimpleRotation(RunParams runParams, RunParamsList botList) : base(runParams)
        {
            CurrentBotIndex = -1;
            BotParams = botList;
            for (int i = 0; i < BotParams.Count; i++)
            {
                BotParams[i].SlaveDriver = true;
            }
        }

        /// <summary>
        /// Handles rotation cycles among the three bots
        /// </summary>
        protected override void ManageBot()
        {
            int timeToRun;

            while (!StopFlag)
            {
                if (!NextBot()) { return; }
                timeToRun = CurrentBot.RandomWorkTime();
                CurrentBot.Start();
                SafeWait(timeToRun);
                CurrentBot.Stop();
            }
        }

        /// <summary>
        /// Chooses the next bot and makes any necessary transitions
        /// </summary>
        /// <returns>true if successful</returns>
        protected virtual bool NextBot()
        {
            if (BotParams.Count <= 1)
            {
                return false;
            }

            int nextBot = CurrentBotIndex;
            while (nextBot == CurrentBotIndex)
            {
                nextBot = RNG.Next(0, BotParams.Count);
            }

            CurrentBotIndex = nextBot;
            if (!SelectBotAction())
            {
                return false;
            }
            CurrentBot = BotRegistry.GetSelectedBot(CurrentBotParams);
            return true;
        }

        /// <summary>
        /// Select the next activity for the next bot to run
        /// </summary>
        /// <returns>true if successful</returns>
        protected virtual bool SelectBotAction()
        {
            return true;
        }
    }
}
