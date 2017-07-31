using RunescapeBot.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RunescapeBot.BotPrograms
{
    public class SimpleRotation : BotProgram
    {
        private const int LOGOUT_CHECK_INTERVAL = 2500;
        public const int NUMBER_OF_BOTS = 3;
        protected RunParamsList BotParamsList;
        public BotProgram CurrentBot;
        public RunParams CurrentRunParams { get { return BotParamsList[BotParamsList.ActiveBot]; } }

        public SimpleRotation(RunParams runParams, RunParamsList botList) : base(runParams)
        {
            BotParamsList = botList;
            BotParamsList.ActiveBot = 0;
            for (int i = 0; i < BotParamsList.Count; i++)
            {
                BotParamsList[i].SlaveDriver = true;
                BotParamsList[i].JagexClient = runParams.JagexClient;
                BotParamsList[i].OSBuddyClient = runParams.OSBuddyClient;
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
                CurrentRunParams.RunUntil = DateTime.Now.AddMilliseconds(timeToRun);
                CurrentBot.Start();
                SafeWait(timeToRun);
                while (!CurrentBot.BotIsDone)
                {
                    SafeWait(LOGOUT_CHECK_INTERVAL);
                }
            }
        }

        /// <summary>
        /// Chooses the next bot and makes any necessary transitions
        /// </summary>
        /// <returns>true if successful</returns>
        protected virtual bool NextBot()
        {
            if (BotParamsList.Count <= 1)
            {
                return false;
            }

            int nextBot = BotParamsList.ActiveBot;
            while (nextBot == BotParamsList.ActiveBot)
            {
                nextBot = RNG.Next(0, BotParamsList.Count);
            }

            BotParamsList.ActiveBot = nextBot;
            if (!SelectBotAction())
            {
                return false;
            }
            CurrentBot = BotRegistry.GetSelectedBot(CurrentRunParams);
            CurrentBot.LogoutWhenDone = true;
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
