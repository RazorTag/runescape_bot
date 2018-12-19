using RunescapeBot.UITools;
using System.Diagnostics;

namespace RunescapeBot.BotPrograms.Popups
{
    class MakeAllSlim : MakeAll
    {
        /// <summary>
        /// Create a record of a Make X popup
        /// </summary>
        /// <param name="xClick">the x-coordinate of the click that opened the Make-X popup</param>
        /// <param name="yClick">the y-coordinate of the click that opened the Make-X popup</param>
        /// <param name="rsClient"></param>
        public MakeAllSlim(int xClick, int yClick, Process rsClient, Keyboard keyboard) : base(xClick, yClick, rsClient, keyboard)
        {

        }

        /// <summary>
        /// Sets the dimensions of the popup
        /// </summary>
        protected override void SetSize()
        {
            Height = 110;
            Width = 100;
        }
    }
}
