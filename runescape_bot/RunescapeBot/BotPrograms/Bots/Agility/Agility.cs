using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RunescapeBot.BotPrograms
{
    public class Agility : BotProgram
    {
        private const int MAX_PASS_OBSTACLE_TRIES = 3;

        public Agility(StartParams startParams) : base(startParams)
        {
            RunParams.Run = true;
        }

        protected delegate bool BasicAction();

        /// <summary>
        /// Tries to pass and verify passage of an agility obstacle
        /// </summary>
        /// <param name="passObstacle">method for passing the obstacle</param>
        /// <param name="verifyPassedObstacle">method for verifying passage of the obstacle</param>
        /// <returns></returns>
        protected bool TryPassObstacle(BasicAction passObstacle, BasicAction verifyPassedObstacle)
        {
            verifyPassedObstacle = verifyPassedObstacle ?? HasPassedObstacle;
            int remainingTries = MAX_PASS_OBSTACLE_TRIES;

            while (remainingTries-- > 0)
            {
                if (passObstacle() && verifyPassedObstacle())
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Default method for verifying passage of an obstacle
        /// </summary>
        /// <returns></returns>
        private bool HasPassedObstacle() { return true; }
    }
}
