﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RunescapeBot.BotPrograms.Bots.MageTrainingArena
{
    public class AlchemistPlayground : BotProgram
    {
        public AlchemistPlayground(RunParams startParams) : base(startParams)
        {
            RunParams.Run = true;
        }
    }
}
