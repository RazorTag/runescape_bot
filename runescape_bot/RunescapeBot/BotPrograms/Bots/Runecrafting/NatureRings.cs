using RunescapeBot.BotPrograms.Settings;
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
        protected NatureRingsSettingsData.FairyRingOptions FairyRingLocation;
        protected NatureRingsSettingsData.GloryOptions GloryType;

        public NatureRings(RunParams startParams) : base(startParams)
        {
            RunParams.Run = true;
            FairyRingLocation = startParams.CustomSettingsData.NatureRings.FairyRing;
            GloryType = startParams.CustomSettingsData.NatureRings.GloryType;
        }

        protected override bool Run()
        {
            //ReadWindow();
            //DebugUtilities.SaveImageToFile(Bitmap, "C:\\Projects\\Roboport\\test_pictures\\nature rings\\test.png");

            return true;
        }

        protected override bool Execute()
        {
            MoveToBank();

            return true;    //assumes success
        }
    }
}
