using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RunescapeBot.BotPrograms
{
    public class ButlerSawmill : BotProgram
    {
        public ButlerSawmill(RunParams startParams) : base(startParams)
        {
            RunParams.Run = true;
            RunParams.LoginWorld = 337;
        }


        protected override bool Run()
        {
            //ReadWindow();
            //DebugUtilities.SaveImageToFile(Bitmap);
            return true;
        }

        protected override bool Execute()
        {
            return true;
        }
    }
}
