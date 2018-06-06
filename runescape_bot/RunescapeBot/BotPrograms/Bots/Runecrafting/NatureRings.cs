using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RunescapeBot.BotPrograms
{
    public class NatureRings : BotProgram
    {
        protected bool ringSlotEmpty;
        protected bool pouchDamaged;

        public NatureRings(RunParams startParams) : base(startParams)
        {
            GetReferenceColors();
            RunParams.Run = true;
        }

        /// <summary>
        /// Sets the filter colors for landmarks
        /// </summary>
        private void GetReferenceColors()
        {

        }

        protected override bool Run()
        {
            //ReadWindow();
            //DebugUtilities.SaveImageToFile(Bitmap, "C:\\Projects\\Roboport\\test_pictures\\nature rings\\test.png");

            return true;
        }

        protected override bool Execute()
        {
            

            return true;    //assumes success
        }
    }
}
