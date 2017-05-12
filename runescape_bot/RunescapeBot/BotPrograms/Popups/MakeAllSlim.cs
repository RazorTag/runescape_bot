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
        public MakeAllSlim(int xClick, int yClick, Process rsClient) : base(xClick, yClick, rsClient)
        {

        }

        /// <summary>
        /// Sets the dimensions of the popup
        /// </summary>
        protected override void SetSize()
        {
            Height = 95;
            TitleHeight = 15;
            Width = 100;
            TitleHash = 31744;
        }
    }
}
