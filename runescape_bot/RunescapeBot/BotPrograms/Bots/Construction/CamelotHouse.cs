using RunescapeBot.BotPrograms.Popups;
using RunescapeBot.Common;
using RunescapeBot.ImageTools;
using RunescapeBot.UITools;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RunescapeBot.BotPrograms
{
    public class CamelotHouse : BotProgram
    {
        protected const double _titleHashPrecision = 0.0001;

        protected Point InventoryLawRuneSlot, BankLawRuneSlot;
        protected RGBHSBRange HousePortalPurple, BankChest, BlueMouseOverText, DemonHead, DialogTitle, DialogBody, ContinueBarBlue, BackpackBrown;

        protected int DialogTitleLeft { get { return 128; } }
        protected int DialogTitleRight { get { return DialogTitleLeft + 370; } }
        protected int DialogTitleTop { get { return ScreenHeight - 142; } }
        protected int DialogTitleBottom { get { return DialogTitleTop + 21; } }

        protected int DialogTextLeft { get { return 128; } }
        protected int DialogTextRight { get { return DialogTextLeft + 370; } }
        protected int DialogTextTop { get { return ScreenHeight - 123; } }
        protected int DialogTextBottom { get { return DialogTextTop + 60; } }

        protected int DialogLeft { get { return 128; } }
        protected int DialogRight { get { return DialogLeft + 370; } }
        protected int DialogTop { get { return ScreenHeight - 142; } }
        protected int DialogBottom { get { return DialogTop + 82; } }

        protected int FailedRuns;
        protected Stopwatch teleportWatch;


        public CamelotHouse(RunParams startParams) : base(startParams)
        {
            FailedRuns = 0;
            RunParams.Run = true;
            RunParams.LoginWorld = 392;
            InventoryLawRuneSlot = new Point(0, 0);
            BankLawRuneSlot = new Point(0, 7);
            SetColors();
            teleportWatch = new Stopwatch();
        }

        private void SetColors()
        {
            BankChest = RGBHSBRangeFactory.BankChest();
            HousePortalPurple = RGBHSBRangeFactory.HousePortalPurple();
            BlueMouseOverText = RGBHSBRangeFactory.MouseoverTextStationaryObject();
            DemonHead = RGBHSBRangeFactory.LesserDemonSkin();
            DialogTitle = RGBHSBRangeFactory.DialogBoxTitle();
            DialogBody = RGBHSBRangeFactory.Black();
            ContinueBarBlue = RGBHSBRangeFactory.GenericColor(Color.Blue);
            BackpackBrown = RGBHSBRangeFactory.BackpackBrown();
        }

        protected override bool Run()
        {
            //ReadWindow();
            //DebugUtilities.SaveImageToFile(Bitmap, "C:\\Projects\\Roboport\\test_pictures\\construction\\enter-amount.png");
            //DebugUtilities.SaveImageToFile(Bitmap, "C:\\Projects\\Roboport\\test_pictures\\test.png");

            //ScreenScraper.BringToForeGround(RSClient);
            //Thread.Sleep(1000);
            //ReadWindow();
            //bool[,] furnaceIcon = ColorFilter(BankChest);
            //DebugUtilities.TestMask(Bitmap, ColorArray, BankChest, furnaceIcon);

            //ReadWindow();
            //bool[,] furnaceIcon = ColorFilter(RGBHSBRangeFactory.LesserDemonSkin());
            //DebugUtilities.TestMask(Bitmap, ColorArray, RGBHSBRangeFactory.LesserDemonSkin(), furnaceIcon);

            //Inventory.OpenSpellbook(false);
            //Inventory.OpenOptions(false);
            //Inventory.TeleportHasRunes(5, 3);
            //CallServant();
            //IsAtHouse();
            //Construct();

            //long titleHash = DialogTitleHash();
            //long dialogHash = DialogHash();
            //AnyDialog();
            //DemonButlerDialog();
            //RepeatLastTaskDialog();
            //SelectAnOptionDialog();
            //WaitingForCommand();
            //UnNoteTheBanknotes();
            //ContinueBar();

            //ReadWindow();
            //Color[,] image = DebugUtilities.LoadImageFromFile("C:\\Projects\\Roboport\\test_pictures\\construction\\select-sawmill.png");
            //Color[,] dialogBody = ImageProcessing.ScreenPiece(image, DialogTextLeft, DialogTextRight, DialogTextTop, DialogTextBottom);
            //bool[,] dialogBodyBinary = ImageProcessing.ColorFilter(dialogBody, RGBHSBRangeFactory.Black());
            //int bodyHash = ImageProcessing.MatchCount(dialogBodyBinary);

            return true;
        }


        protected override bool Execute()
        {
            if (FailedRuns >= 5)
            {
                return false;
            }

            if (Bank() && TeleportToHouse() && Construct())
            {
                FailedRuns = 0;
            }
            else
            {
                FailedRuns++;   //TODO teleport to camelot / refresh bank / relog / visit GE
                GeneralTroubleShoot();
            }
            if (StopFlag) { return false; }

            if (!Reset())
            {
                return false;   //TODO restock at the GE
            }
            
            return true;
        }

        /// <summary>
        /// Looks for and fixes common issues
        /// </summary>
        protected virtual void GeneralTroubleShoot()
        {
            ReadWindow();

            Bank bank = new Bank(RSClient);
            if (bank.BankIsOpen())
            {
                bank.CloseBank();
                if (SafeWait(1000)) { return; }
            }

            if (HouseOptionsIsOpen())
            {
                LeftClick(ScreenWidth - 40, ScreenHeight - 282);    //close out of House Options
                if (SafeWait(1000)) { return; }
            }
        }

        /// <summary>
        /// Moves the bot back to its Camelot starting position
        /// </summary>
        /// <returns></returns>
        protected bool Reset()
        {
            if (Inventory.StandardTeleport(Inventory.StandardTeleports.Camelot, false))
            {
                teleportWatch.Restart();
                return true;
            }
            return false;   //TODO restock at the GE
        }

        /// <summary>
        /// Teleports to the player's house and waits to verify arrival
        /// </summary>
        /// <returns></returns>
        protected bool TeleportToHouse()
        {
            if (Inventory.StandardTeleport(Inventory.StandardTeleports.House, false))
            {
                teleportWatch.Restart();
                return true;
            }
            return false;
        }

        /// <summary>
        /// Waits until an ongoing teleport is completed
        /// </summary>
        /// <returns>true if the stopflag is raised</returns>
        protected bool WaitForTeleport()
        {
            if (teleportWatch.IsRunning)
            {
                teleportWatch.Stop();
                return (SafeWait(Inventory.TELEPORT_DURATION - teleportWatch.ElapsedMilliseconds));
            }
            return false;
        }

        /// <summary>
        /// Determines if the player is next to a house portal
        /// </summary>
        /// <returns></returns>
        protected bool IsAtHouse()
        {
            ReadWindow();
            bool[,] portal = ColorFilterPiece(HousePortalPurple, Center, 200);
            double match = ImageProcessing.FractionalMatch(portal);
            return match > 0.001;
        }

        /// <summary>
        /// Do activities in a player owned house before returning to Camelot
        /// </summary>
        /// <returns>true if successful</returns>
        protected virtual bool Construct()
        {
            return true;
        }

        /// <summary>
        /// Swap items out at the bank chest
        /// </summary>
        /// <returns></returns>
        protected virtual bool Bank()
        {
            return true;
        }

        /// <summary>
        /// Selects call servant from the house options
        /// </summary>
        /// <returns>true if successful</returns>
        protected bool CallServant()
        {
            Point houseOptions = HouseOptionsLocation();
            Stopwatch watch = new Stopwatch();
            watch.Start();
            while (!StopFlag && !HouseOptionsIsOpen())
            {
                if (watch.ElapsedMilliseconds > 2000)
                {
                    if (SafeWait(500)) { return false; }
                    if (HouseOptionsIsOpen())
                    {
                        LeftClick(houseOptions.X, ScreenHeight - 76, 12);    //click on call servant
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                LeftClick(houseOptions.X, houseOptions.Y, 10);    //click on house options
            }
            LeftClick(houseOptions.X, ScreenHeight - 76, 12);    //click on call servant
            return true;
        }

        /// <summary>
        /// Gets the location of the House Options button within the Options tab
        /// </summary>
        /// <returns>the location of the middle of House Options</returns>
        protected Point HouseOptionsLocation()
        {
            return new Point(ScreenWidth - 97, ScreenHeight - 58);
        }

        /// <summary>
        /// Attempts to safely unnote an item using the bank chest
        /// </summary>
        /// <param name="inventorySlot"></param>
        /// <returns></returns>
        protected bool UnNoteBankChest(Point inventorySlot, bool noteSelected)
        {
            if (!noteSelected)
            {
                Inventory.ClickInventory(inventorySlot);
            }

            Point bankChestClick;
            if (BankChestClickLocation(out bankChestClick))
            {
                Mouse.MoveMouse(bankChestClick.X, bankChestClick.Y, RSClient);
                if (WaitForMouseOverText(BlueMouseOverText))
                {
                    Mouse.LeftClick(bankChestClick.X, bankChestClick.Y, RSClient);
                    if (WaitFor(UnNoteTheBanknotes))
                    {
                        Keyboard.WriteNumber(1);
                        return true;
                    }
                    else
                    {
                        RefreshInventory();
                        return false;
                    }
                }
            }

            Inventory.ClickInventory(inventorySlot);
            return false;
        }

        /// <summary>
        /// Refreshes the player's inventory to a working state
        /// </summary>
        /// <returns>true if successful</returns>
        protected virtual bool RefreshInventory()
        {
            return true;
        }

        /// <summary>
        /// Determines if the inventory look like it is ready
        /// </summary>
        /// <returns>true if the inventory appears to be set up correctly</returns>
        protected virtual bool InventoryIsReady()
        {
            return true;
        }

        /// <summary>
        /// Finds the location of the bank chest to click on
        /// </summary>
        /// <param name="clickLocation"></param>
        /// <returns></returns>
        protected bool BankChestClickLocation(out Point clickLocation)
        {
            Blob bankChest;
            if (LocateStationaryObject(BankChest, out bankChest, 0, 5000, 1, int.MaxValue, FindBankChest))
            {
                Point clickOffset = new Point(-ArtifactLength(0.012), -ArtifactLength(0.006));
                clickLocation = Geometry.AddPoints(bankChest.Center, clickOffset);
                clickLocation = Probability.GaussianCircle(clickLocation, 1, 0, 360, 3);
                return true;
            }
            clickLocation = new Point(0, 0);
            return false;
        }

        /// <summary>
        /// Searches for the bank chest in an area around the player
        /// </summary>
        /// <param name="stationaryObject">color filter for the bank chest brown wood</param>
        /// <param name="foundObject">bank chest blob return value</param>
        /// <param name="minimumSize">not used</param>
        /// <param name="maximumSize">not used</param>
        /// <returns></returns>
        protected bool FindBankChest(RGBHSBRange stationaryObject, out Blob foundObject, int minimumSize, int maximumSize)
        {
            int widthRadius = ArtifactLength(0.35);
            int heightRadius = ArtifactLength(0.25);
            int left = Center.X - widthRadius;
            int right = Center.X + widthRadius;
            int top = Center.Y - heightRadius;
            int bottom = Center.Y + heightRadius;
            return LocateObject(BankChest, out foundObject, left, right, top, bottom, ArtifactSize(0.0004), ArtifactSize(0.0015));
        }

        /// <summary>
        /// Determines if house options is open
        /// </summary>
        /// <returns>true if the world switcher is open</returns>
        protected bool HouseOptionsIsOpen()
        {
            Inventory.OpenOptions(false);
            ReadWindow();
            int left = ScreenWidth - 169;
            int right = left + 100;
            int top = ScreenHeight - 296;
            int bottom = top + 20;
            Color[,] houseOptionsTitle = ScreenPiece(left, right, top, bottom);
            double houseOptionsMatch = ImageProcessing.FractionalMatch(houseOptionsTitle, RGBHSBRangeFactory.BankTitle());
            const double houseOptionsMinimumMatch = 0.05;
            return houseOptionsMatch > houseOptionsMinimumMatch;
        }

        /// <summary>
        /// Waits for the specified dialog to appear
        /// </summary>
        /// <param name="dialog">dialog check delegate</param>
        /// <returns>true if the dialog is showing</returns>
        protected bool WaitFor(IsTrue dialog, int timeout = 5000)
        {
            Stopwatch watch = new Stopwatch();
            watch.Start();
            while (!StopFlag && watch.ElapsedMilliseconds < timeout)
            {
                if (dialog())
                {
                    return true;
                }
                SafeWait(200);
            }
            return false;
        }
        protected delegate bool IsTrue();


        /// <summary>
        /// Calculates a color hash of the title of the dialog box
        /// </summary>
        /// <returns></returns>
        protected long DialogTitleHash()
        {
            ReadWindow();
            Color[,] dialogTitle = ScreenPiece(DialogTitleLeft, DialogTitleRight, DialogTitleTop, DialogTitleBottom);
            return ImageProcessing.ColorSum(dialogTitle);
        }

        /// <summary>
        /// Calculates the number of text pixels in the dialog body
        /// </summary>
        /// <returns>the number of pixels of text in the dialog body</returns>
        protected int DialogBodyText()
        {
            ReadWindow();
            bool[,] dialogBody = ColorFilterPiece(DialogBody, DialogTextLeft, DialogTextRight, DialogTextTop, DialogTextBottom);
            return ImageProcessing.MatchCount(dialogBody);
        }

        /// <summary>
        /// Calculates a hash of the dialog title and body sections
        /// </summary>
        /// <returns></returns>
        protected long DialogHash(bool readWindow = true)
        {
            if (readWindow)
            {
                ReadWindow();
            }
            Color[,] dialogTitle = ScreenPiece(DialogLeft, DialogRight, DialogTop, DialogBottom);
            return ImageProcessing.ColorSum(dialogTitle);
        }

        /// <summary>
        /// Determines if "Click here to continue is showing"
        /// </summary>
        /// <returns>true if the player can push space to continue a dialog</returns>
        protected bool ContinueBar()
        {
            int left = 233;
            int right = left + 144;
            int top = ScreenHeight - 59;
            int bottom = top + 10;
            bool[,] continueBar = ColorFilterPiece(ContinueBarBlue, left, right, top, bottom);
            int textSize = ImageProcessing.MatchCount(continueBar);
            return Numerical.CloseEnough(285, textSize, 0.01);
        }

        /// <summary>
        /// Determines if an NPC dialog is in the dialog box
        /// </summary>
        /// <returns>true if an NPC dialog shows up</returns>
        protected bool AnyDialog(bool readWindow = true)
        {
            if (readWindow && !ReadWindow())
            {
                return false;
            }
            Color[,] dialogTitle = ScreenPiece(DialogTitleLeft, DialogTitleRight, DialogTitleTop, DialogTitleBottom);
            double dialogMatch = ImageProcessing.FractionalMatch(dialogTitle, DialogTitle);
            return dialogMatch > 0.005;
        }

        /// <summary>
        /// Determines if an NPC dialog is in the dialog box
        /// </summary>
        /// <returns>true if an NPC dialog shows up</returns>
        protected bool AnyDialog()
        {
            return AnyDialog(true);
        }

        /// <summary>
        /// Determines if the dialog box is showing for the "Un-note the banknotes?" title
        /// </summary>
        /// <returns></returns>
        protected bool UnNoteTheBanknotes()
        {
            long titleHash = DialogTitleHash();
            return Numerical.CloseEnough(4090128, titleHash, _titleHashPrecision);
        }
    }
}
