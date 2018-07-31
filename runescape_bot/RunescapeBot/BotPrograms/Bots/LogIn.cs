using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RunescapeBot.BotPrograms
{
    public class LogInToGame : BotProgram
    {
        public LogInToGame(RunParams startParams) : base(startParams)
        {
            RunParams.DefaultCameraPosition = RunParams.CameraPosition.AsIs;
        }

        /// <summary>
        /// Determines if the user is logged out and logs him back in if he is.
        /// If the bot does not have valid login information, then it will quit.
        /// </summary>
        /// <returns>true if we are already logged in or we are able to log in, false if we can't log in</returns>
        protected override bool CheckLogIn(bool readWindow)
        {
            if (!IsLoggedOut())
            {
                return true;    //already logged in
            }

            //see if we have login and password to log in
            if (string.IsNullOrEmpty(RunParams.Login) || string.IsNullOrEmpty(RunParams.Password))
            {
                MessageBox.Show("Cannot log in without login information");
                return false;
            }
            else
            {
                return LogIn();
            }
        }
    }
}
