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
        private const int LOGOUT_CHECK_INTERVAL = 500;
        public const int NUMBER_OF_BOTS = 3;
        protected RunParamsList BotParamsList;
        public BotProgram CurrentBot;
        public RunParams CurrentRunParams { get { return BotParamsList[BotParamsList.ActiveBot]; } }

        public SimpleRotation(RunParams runParams, RunParamsList botList) : base(runParams)
        {
            BotParamsList = botList;
            BotParamsList.ActiveBot = -1;
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
                if (!NextBot())
                {
                    return;
                }

                timeToRun = RandomWorkTime();
                CurrentRunParams.RunUntil = DateTime.Now.AddMilliseconds(timeToRun);

                //Don't actually run a bot without login info
                if (string.IsNullOrEmpty(CurrentRunParams.Login) || string.IsNullOrEmpty(CurrentRunParams.Password))
                {
                    CurrentRunParams.ActiveBot.BotState = BotState.Running;
                    CurrentRunParams.SetNewState(timeToRun);
                    CurrentRunParams.BotIdle = true;
                    SafeWait(timeToRun);
                    CurrentRunParams.BotIdle = false;
                }
                else
                {
                    CurrentBot.Start();
                    SafeWait(timeToRun);
                    while (!CurrentBot.BotIsDone)
                    {
                        SafeWait(LOGOUT_CHECK_INTERVAL);
                    }
                }

                if (!StopFlag)
                {
                    RSClient.PrepareClient(Screen, true);    //restart the client to get a random new world
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
            do {
                nextBot = RNG.Next(0, BotParamsList.Count);
            } while (nextBot == BotParamsList.ActiveBot);
            BotParamsList.ActiveBot = nextBot;

            if (SelectBotAction())
            {
                CurrentBot = BotRegistry.GetSelectedBot(CurrentRunParams);
                CurrentBot.LogoutWhenDone = true;
            }
            else
            {
                CurrentBot = null;
            }
            
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
