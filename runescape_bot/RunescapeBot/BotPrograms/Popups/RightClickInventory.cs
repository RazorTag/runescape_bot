using System.Diagnostics;

namespace RunescapeBot.BotPrograms.Popups
{
    public class RightClickInventory : RightClick
    {
        /// <summary>
        /// Create a record of a standard Make X popup
        /// </summary>
        /// <param name="xClick">the x-coordinate of the click that opened the Make-X popup</param>
        /// <param name="yClick">the y-coordinate of the click that opened the Make-X popup</param>
        /// <param name="rsClient"></param>
        public RightClickInventory(int xClick, int yClick, Process rsClient) : base(xClick, yClick, rsClient) { }

        /// <summary>
        /// Sets the dimensions of the popup
        /// </summary>
        protected override void SetSize()
        {
            Height = 82;
            TitleHeight = 16;
            Width = 136;
        }

        /// <summary>
        /// Click the Make-1 option in a Make-X pop-up
        /// </summary>
        public void Use()
        {
            const int yOffset = 25;
            SelectOption(yOffset);
        }

        /// <summary>
        /// Click the Make-5 option in a Make-X pop-up
        /// </summary>
        public void DropItem()
        {
            const int yOffset = 40;
            SelectOption(yOffset, 15);
        }

        /// <summary>
        /// Click the Make-10 option in a Make-X pop-up
        /// </summary>
        public void Examine()
        {
            const int yOffset = 55;
            SelectOption(yOffset);
        }
    }
}
