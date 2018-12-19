using RunescapeBot.Common;
using RunescapeBot.ImageTools;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RunescapeBot.BotPrograms
{
    public class RSClient
    {
        /// <summary>
        /// Operating system process for the game client.
        /// </summary>
        public Process Value { get; set; }


        private GameScreen Screen;

        /// <summary>
        /// Defines how the bot should be run.
        /// </summary>
        private RunParams RunParams;

        public RSClient(RunParams runParams)
        {
            RunParams = runParams;
            Value = ScreenScraper.GetClient();
            ScreenScraper.RSClient = this;
        }

        public void AddScreen(GameScreen screen)
        {
            Screen = screen;
        }

        /// <summary>
        /// Implicitly use the Process Value of RSClient when needed.
        /// </summary>
        /// <param name="client">this RSClient</param>
        public static implicit operator Process(RSClient client)
        {
            return client.Value;
        }

        #region methods

        /// <summary>
        /// Makes sure that a client is running and starts it if it isn't
        /// </summary>
        /// <param name="forceRestart">Set to true to force a client restart even if the client is already running</param>
        /// <returns>true if client is successfully prepared</returns>
        public bool PrepareClient(bool forceRestart = false)
        {
            if (!forceRestart && ScreenScraper.ProcessExists(Value)) { return true; }

            Process client = null;
            Stopwatch longWatch = new Stopwatch();
            longWatch.Start();
            while (longWatch.ElapsedMilliseconds < UnitConversions.HoursToMilliseconds(24) && !BotProgram.StopFlag)
            {
                if (!ScreenScraper.RestartClient(ref client, RunParams.RuneScapeClient, RunParams.ClientFlags))
                {
                    BotProgram.SafeWait(5000);
                    continue;
                }
                //Successful restart
                Value = client;

                Stopwatch watch = new Stopwatch();
                watch.Start();
                //Wait for cient to be visually recognized.
                do
                {
                    BotProgram.SafeWait(UnitConversions.SecondsToMilliseconds(5));
                    if (Screen.ReadWindow(false) && (Screen.IsLoggedOut(false) || Screen.IsLoggedIn(false)))
                    {
                        return true;
                    }
                }
                while ((watch.ElapsedMilliseconds < UnitConversions.MinutesToMilliseconds(5)) && !BotProgram.StopFlag);
            }

            if (!BotProgram.StopFlag)
            {
                const string errorMessage = "Client did not start correctly";
                MessageBox.Show(errorMessage);
            }

            Value = null;
            return false;
        }

        #endregion
    }
}
