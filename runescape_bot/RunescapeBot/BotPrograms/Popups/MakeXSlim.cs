using System.Diagnostics;

namespace RunescapeBot.BotPrograms.Popups
{
    /// <summary>
    /// Choose Option popup that contains the options Make-1, Make-5, Make-10, and Make-X
    /// -fletching logs into arrows shafts, short bows, long bows, or corssbow stocks
    /// </summary>
    public class MakeXSlim : MakeX
    {
        /// <summary>
        /// Create a record of a Make X popup
        /// </summary>
        /// <param name="xClick">the x-coordinate of the click that opened the Make-X popup</param>
        /// <param name="yClick">the y-coordinate of the click that opened the Make-X popup</param>
        /// <param name="rsClient"></param>
        public MakeXSlim(int xClick, int yClick, Process rsClient) : base(xClick, yClick, rsClient) { }

        /// <summary>
        /// Sets the dimensions of the popup
        /// </summary>
        protected override void SetSize()
        {
            Height = 95;
            Width = 100;
        }
    }
}
