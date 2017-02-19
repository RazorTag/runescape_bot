using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WindowsFormsApplication1.BotPrograms;

namespace WindowsFormsApplication1
{
    public partial class Start : Form
    {
        private ArrayList runningBots;

        public enum BotActions : int {
                [Description("Gold Bracelets")]
                GoldBracelets,
                [Description("Lesser Demon")]
                LesserDemon
            };

        public string GetDescription(Enum value)
        {
            Type type = value.GetType();
            string name = Enum.GetName(type, value);
            if (name != null)
            {
                FieldInfo field = type.GetField(name);
                if (field != null)
                {
                    DescriptionAttribute attr =
                           Attribute.GetCustomAttribute(field,
                             typeof(DescriptionAttribute)) as DescriptionAttribute;
                    if (attr != null)
                    {
                        return attr.Description;
                    }
                }
            }
            return null;
        }

        public Start()
        {
            InitializeComponent();
            Array actions = Enum.GetValues(typeof(BotActions));
            string[] names = new string[actions.Length];
            for (int i = 0; i < actions.Length; i++)
            {
                names[i] = GetDescription((Enum) actions.GetValue(i));
            }
            BotActionSelect.DataSource = names;
            runningBots = new ArrayList();
        }

        private void StartButton_Click(object sender, EventArgs e)
        {
            StartParams startParams = CollectStartParams();
            BotProgram botProgram;

            switch ((BotActions)BotActionSelect.SelectedIndex)
            {
                case BotActions.GoldBracelets:
                    botProgram = new GoldBracelets(startParams);
                    break;

                case BotActions.LesserDemon:
                    botProgram = new LesserDemon(startParams);
                    break;

                default:
                    botProgram = null;
                    break;
            }

            RunBotProgram(botProgram);
        }

        private StartParams CollectStartParams()
        {
            StartParams startParams = new StartParams();
            startParams.username = Username.Text;

            return startParams;
        }

        private void RunBotProgram(BotProgram botProgram)
        {
            if (botProgram == null) { return; }
            botProgram.Start(runningBots);
        }
    }
}
