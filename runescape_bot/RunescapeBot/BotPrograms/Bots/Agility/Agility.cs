using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RunescapeBot.BotPrograms
{
    public class Agility : BotProgram
    {
        protected int MaxPassObstacleTries;

        public Agility(RunParams startParams) : base(startParams)
        {
            RunParams.Run = true;
            MaxPassObstacleTries = 3;
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
            int remainingTries = MaxPassObstacleTries;

            while (remainingTries-- > 0)
            {
                if (passObstacle() && verifyPassedObstacle())
                {
                    return true;
                }
                if (StopFlag) { return false; }
            }
            return false;
        }

        /// <summary>
        /// Attempts to clear each of a list of obstacles.
        /// Stops trying after an obstacle is failed.
        /// </summary>
        /// <param name="obstacles">list of pass and verify methods for a series of obstacles</param>
        /// <returns>True if successful for all obstacles. False if we fail on any obstacle.</returns>
        protected bool TryPassObstacles(List<Tuple<BasicAction, BasicAction>> obstacles)
        {
            foreach(Tuple<BasicAction, BasicAction> obstacle in obstacles)
            {
                if (StopFlag) { return false; }

                BasicAction tryPass = obstacle.Item1;
                BasicAction verifyPassed = obstacle.Item2;
                if (!TryPassObstacle(tryPass, verifyPassed))
                {
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// Default method for verifying passage of an obstacle
        /// </summary>
        /// <returns></returns>
        private bool HasPassedObstacle() { return true; }
    }
}
