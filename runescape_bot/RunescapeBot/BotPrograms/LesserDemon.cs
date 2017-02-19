namespace WindowsFormsApplication1.BotPrograms
{
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
