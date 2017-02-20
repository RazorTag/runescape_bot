namespace WindowsFormsApplication1.BotPrograms
{
    /// <summary>
    /// Targets the lesser demon trapped in the Wizards' Tower
    /// </summary>
    class LesserDemon : BotProgram
    {
        public LesserDemon(StartParams startParams) : base(startParams) { }

        protected override void Run()
        {
            System.Windows.Forms.MessageBox.Show("LesserDemon running");

            Done();
        }
    }
}
