namespace RunescapeBot.BotPrograms
{
    /// <summary>
    /// Smiths gold bracelets at the furnace in Port Phasmatys
    /// </summary>
    public class GoldBracelets : BotProgram
    {
        public GoldBracelets(StartParams startParams) : base(startParams) { }

        protected override void Run()
        {
            System.Windows.Forms.MessageBox.Show("GoldBracelets running");
        }
    }
}
