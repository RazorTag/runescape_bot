using System.Drawing;

namespace RunescapeBot.BotPrograms
{
    public class UnfinishedPotions : Herblore
    {
        /// <param name="startParams"></param>
        /// <param name="craftingTime">time needed to make the 14 items being crafted</param>
        public UnfinishedPotions(StartParams startParams) : base(startParams, MAKE_UNFINISHED_POTION_TIME) { }

        /// <summary>
        /// Clean the grimy herbs before making the unfinished potions
        /// </summary>
        /// <returns>true if successful</returns>
        protected override bool PreMake()
        {
            return CleanHerbs();
        }
    }
}
