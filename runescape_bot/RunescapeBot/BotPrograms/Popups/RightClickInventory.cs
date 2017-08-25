using System.Diagnostics;

namespace RunescapeBot.BotPrograms.Popups
{
    public class RightClickInventory : RightClick
    {
        private int[] ExtraOptions;

        /// <summary>
        /// Create a record of a standard Make X popup
        /// </summary>
        /// <param name="xClick">the x-coordinate of the click that opened the Make-X popup</param>
        /// <param name="yClick">the y-coordinate of the click that opened the Make-X popup</param>
        /// <param name="rsClient">RS client process</param>
        /// <param name="exraOptions">specifies the order of extra options in the right-click menu</param>
        public RightClickInventory(int xClick, int yClick, Process rsClient, int[] extraOptions = null) : base(xClick, yClick, rsClient)
        {
            ExtraOptions = extraOptions;
            SetSize();
            AdjustPosition();
        }

        /// <summary>
        /// Sets the dimensions of the popup
        /// </summary>
        protected override void SetSize()
        {
            Height = 82;
            if (ExtraOptions != null) { Height += (ROW_HEIGHT * ExtraOptions.Length); }
            Width = 100;    //TODO dynamically calculate width since different items produce different widths
        }

        /// <summary>
        /// Click the Make-1 option in a Make-X pop-up
        /// </summary>
        public void Use()
        {
            const int normalIndex = 0;
            int yOffset = 25 + (OptionsAbove(normalIndex) * ROW_HEIGHT);
            SelectOption(yOffset, 15);
        }

        /// <summary>
        /// Click the Make-5 option in a Make-X pop-up
        /// </summary>
        public void DropItem()
        {
            const int normalIndex = 1;
            int yOffset = 40 + (OptionsAbove(normalIndex) * ROW_HEIGHT);
            SelectOption(yOffset, 15);
        }

        /// <summary>
        /// Click the Make-10 option in a Make-X pop-up
        /// </summary>
        public void Examine()
        {
            const int normalIndex = 2;
            int yOffset = 55 + (OptionsAbove(normalIndex) * ROW_HEIGHT);
            SelectOption(yOffset, 15);
        }

        /// <summary>
        /// Determines the number of extra options that are listed above the specified option
        /// </summary>
        /// <param name="extraOptions"></param>
        /// <param name="normalIndex"></param>
        /// <returns>the number of extra options that displace the selected option downward</returns>
        private int OptionsAbove(int normalIndex)
        {
            if (ExtraOptions == null) { return 0; }

            int extraOptionsAbove = 0;
            for (int i = 0; i < ExtraOptions.Length; i++)
            {
                if (ExtraOptions[i] <= normalIndex)
                {
                    extraOptionsAbove++;
                }
            }
            return extraOptionsAbove;
        }
    }
}
