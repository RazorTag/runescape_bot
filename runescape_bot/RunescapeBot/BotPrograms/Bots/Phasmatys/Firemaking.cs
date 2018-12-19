using RunescapeBot.BotPrograms.Popups;
using RunescapeBot.ImageTools;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RunescapeBot.BotPrograms
{
    public class Firemaking : BankPhasmatys
    {
        public const int SET_FIRE_TIME = 4 * BotRegistry.GAME_TICK;
        RGBHSBRange BridgeIcon = RGBHSBRangeFactory.BridgeIcon();
        protected int BridgeIconMinSize;
        protected int FireLine;
        protected Point Tinderbox;  //location of the tinderbox in the inventory


        public Firemaking(RunParams startParams) : base(startParams)
        {
            BankIconMinSize = 50;
            BridgeIconMinSize = 100;
            FireLine = 1;
            Tinderbox = new Point(0, 0);
        }


        protected override bool Run()
        {
            //ReadWindow();
            //DebugUtilities.SaveImageToFile(Bitmap);

            //ReadWindow();
            //bool[,] bankIcon = ColorFilter(BankIconDollar);
            //DebugUtilities.TestMask(Bitmap, ColorArray, BankIconDollar, bankIcon, "C:\\Projects\\Roboport\\test_pictures\\mask_tests\\", "bankIcon");

            //ReadWindow();
            //bool[,] thing = ColorFilter(BridgeIcon);
            //DebugUtilities.TestMask(Bitmap, ColorArray, BridgeIcon, thing, "C:\\Projects\\Roboport\\test_pictures\\mask_tests\\", "bridgeIcon");

            //ReadWindow();
            //Point moveTowardBank = Minimap.RadialClickLocation(350, 0.95);
            //LeftClick(moveTowardBank.X, moveTowardBank.Y);

            //ReadWindow();
            //MoveToFireLine();

            //ReadWindow();
            //SetFires();

            return true;
        }

        protected override bool Execute()
        {
            //Move to the bank and open it
            if (!Banking.MoveToBank(7000, true, BankIconMinSize))
            {
                if (StopFlag) { return false; }

                Point moveTowardBank = Minimap.RadialToRectangular(350, 1);
                LeftClick(moveTowardBank.X, moveTowardBank.Y);
                if (SafeWait(6000))
                {
                    return false;
                }
                Vision.WaitDuringPlayerAnimation(10000);
                if (!MoveToBankPhasmatys(2700))
                {
                    return false;
                }
            }
            if (StopFlag || !ClickBankBooth())
            {
                return false;
            }

            if (StopFlag) { return false; }
            BankItems();

            if (!MoveToFireLine() || StopFlag)
            {
                return false;
            }
            if (SafeWait(5000)) { return false; }
            Vision.WaitDuringPlayerAnimation(10000);

            if (!SetFires() || StopFlag)
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Sets all of the inventory logs on fire
        /// </summary>
        /// <returns>true if successful</returns>
        protected bool SetFires()
        {
            for (int i = 1; i < Math.Min(RunParams.Iterations + 1, Inventory.INVENTORY_CAPACITY); i++)
            {
                Inventory.ClickInventory(Tinderbox.X, Tinderbox.Y, false);
                Inventory.ClickInventory(i);
                if (SafeWait(SET_FIRE_TIME)) { return false; }
                RunParams.Iterations--;
            }
            return true;
        }

        /// <summary>
        /// Moves to the extreme right side of the straight line for fire
        /// </summary>
        /// <returns>true if successful</returns>
        protected bool MoveToFireLine()
        {
            Screen.ReadWindow();
            Blob bridge = Minimap.LocateObject(BridgeIcon, BridgeIconMinSize);
            if (bridge == null)
            {
                return false;
            }

            //bottom-right corner grid square of the bridge on the minimap
            int x = bridge.RightBound;
            int y = bridge.BottomBound;
            Point click = new Point(x, y);
            click = Minimap.MinimapToScreenCoordinates(click);

            click.Y += FireLine * MinimapGauge.GRID_SQUARE_SIZE;
            NextFireLine();
            LeftClick(click.X, click.Y);

            return true;
        }

        /// <summary>
        /// Switch between fire lanes 1 and 3
        /// </summary>
        protected void NextFireLine()
        {
            if (FireLine == 1)
            {
                FireLine = 3;
            }
            else
            {
                FireLine = 1;
            }
        }

        /// <summary>
        /// Refresh inventory
        /// </summary>
        protected void BankItems()
        {
            Bank bankPopup = new Bank(RSClient, Inventory, Keyboard);
            bankPopup.DepositInventory();
            SafeWaitPlus(500, 200);
            bankPopup.WithdrawOne(7, 0);
            SafeWaitPlus(500, 200);
            bankPopup.WithdrawAll(6, 0);
            bankPopup.Close();
        }
    }
}
