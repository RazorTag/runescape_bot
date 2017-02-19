namespace WindowsFormsApplication1
{
    public class GoldBracelets : BotProgram
    {
        public GoldBracelets(StartParams startParams) : base(startParams) { }

        protected override void Run()
        {
            System.Windows.Forms.MessageBox.Show("GoldBracelets running");

            Done();
        }
    }
}
