using RunescapeBot.Common;
using RunescapeBot.ImageTools;
using RunescapeBot.UITools;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RunescapeBot.BotPrograms
{
    public class CamelotHouse : BotProgram
    {
        protected const double _titleHashPrecision = 0.0001;

        protected Point InventoryLawRuneSlot, BankLawRuneSlot;
        protected RGBHSBRange HousePortalPurple, BankChest, BlueMouseOverText, DemonHead, DialogTitle, DialogBody;

        protected int DialogTitleLeft { get { return 128; } }
        protected int DialogTitleRight { get { return DialogTitleLeft + 370; } }
        protected int DialogTitleTop { get { return ScreenHeight - 142; } }
        protected int DialogTitleBottom { get { return DialogTitleTop + 21; } }

        protected int DialogTextLeft { get { return 128; } }
        protected int DialogTextRight { get { return DialogTextLeft + 370; } }
        protected int DialogTextTop { get { return ScreenHeight - 123; } }
        protected int DialogTextBottom { get { return DialogTextTop + 60; } }


        public CamelotHouse(RunParams startParams) : base(startParams)
        {
            RunParams.Run = true;
            RunParams.LoginWorld = 392;
            InventoryLawRuneSlot = new Point(0, 0);
            BankLawRuneSlot = new Point(0, 7);
            BankChest = RGBHSBRangeFactory.BankChest();
            HousePortalPurple = RGBHSBRangeFactory.HousePortalPurple();
            BlueMouseOverText = RGBHSBRangeFactory.MouseoverTextStationaryObject();
            DemonHead = RGBHSBRangeFactory.LesserDemonSkin();
            DialogTitle = RGBHSBRangeFactory.DialogBoxTitle();
            DialogBody = RGBHSBRangeFactory.DialogBoxBody();
        }

        protected override bool Run()
        {
            ReadWindow();
            //DebugUtilities.SaveImageToFile(Bitmap, "C:\\Projects\\Roboport\\test_pictures\\construction\\test.png");

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

            //AnyDialog();
            //DemonButlerDialog();
            //RepeatLastTaskDialog();
            //SelectAnOptionDialog();
            //WaitingForCommand();
            UnNoteTheBanknotes();

            return true;
        }


        protected override bool Execute()
        {
            if (!Bank())
            {
                return false;
            }
            if (!TeleportToHouse())
            {
                return false;
            }
            if (!Construct())
            {
                return false;
            }
            if (!Inventory.StandardTeleport(Inventory.StandardTeleports.Camelot))
            {
                return false;   //TODO restock at the GE
            }

            RunParams.Iterations--;
            return true;
        }

        /// <summary>
        /// Teleports to the player's house and waits to verify arrival
        /// </summary>
        /// <returns></returns>
        protected bool TeleportToHouse()
        {
            if (!Inventory.StandardTeleport(Inventory.StandardTeleports.House))
            {
                return false;
            }
            Stopwatch watch = new Stopwatch();
            watch.Start();
            while (!IsAtHouse())
            {
                if (watch.ElapsedMilliseconds > 10000)
                {
                    return false;
                }
            }
            return true;
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
            Inventory.OpenOptions(false);
            int x = ScreenWidth - 97;
            LeftClick(x, ScreenHeight - 58, 12);    //click on house options

            Stopwatch watch = new Stopwatch();
            watch.Start();
            while (!HouseOptionsIsOpen() && !StopFlag)
            {
                SafeWait(200);
                if (watch.ElapsedMilliseconds > 5000)
                {
                    return false;
                }
            }
            LeftClick(x, ScreenHeight - 76, 12);    //click on call servant
            return true;
        }

        /// <summary>
        /// Attempts to safely unnote an item using the bank chest
        /// </summary>
        /// <param name="inventorySlot"></param>
        /// <returns></returns>
        protected bool UnNoteBankChest(Point inventorySlot)
        {
            Blob bankChest;
            if (LocateObject(BankChest, out bankChest, 1))
            {
                Inventory.ClickInventory(inventorySlot);
                Point clickOffset = new Point(-ArtifactLength(0.012), 0);
                Point bankChestClick = Geometry.AddPoints(bankChest.Center, clickOffset);
                bankChestClick = Probability.GaussianCircle(bankChestClick, 5, 0, 360, 10);
                Mouse.MoveMouse(bankChestClick.X, bankChestClick.Y, RSClient);
                if (WaitForMouseOverText(BlueMouseOverText))
                {
                    Mouse.LeftClick(bankChestClick.X, bankChestClick.Y, RSClient, 0);
                    if (!WaitForDialog(UnNoteTheBanknotes))
                    {
                        return false;
                    }
                    Keyboard.WriteNumber(1);
                    return true;
                }
            }
            return false;
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
        /// Goes through the dialog to pay the butler
        /// Assumes you are already on the dialog of the butler asking for money
        /// </summary>
        /// <returns>true if successful</returns>
        protected bool PayButler()
        {
            Keyboard.Space();   //you owe me money

            if (StopFlag || !WaitForDialog(SelectAnOptionDialog))
            {
                return false;
            }
            Keyboard.WriteNumber(1);

            if (StopFlag || !WaitForDialog(DemonButlerDialog))   //thank you for money
            {
                return false;
            }
            Keyboard.Space();

            return true;
        }

        /// <summary>
        /// Waits for the specified dialog to appear
        /// </summary>
        /// <param name="dialog">dialog check delegate</param>
        /// <returns>true if the dialog is showing</returns>
        protected bool WaitForDialog(DialogIsShowing dialog, int timeout = 5000)
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
        protected delegate bool DialogIsShowing();


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
            bool[,] dialogTitle = ColorFilterPiece(DialogBody, DialogTextLeft, DialogTextRight, DialogTextTop, DialogTextBottom);
            return ImageProcessing.MatchCount(dialogTitle);
        }

        /// <summary>
        /// Determines if an NPC dialog is in the dialog box
        /// </summary>
        /// <returns>true if an NPC dialog shows up</returns>
        protected bool AnyDialog()
        {
            ReadWindow();
            Color[,] dialogTitle = ScreenPiece(DialogTitleLeft, DialogTitleRight, DialogTitleTop, DialogTitleBottom);
            double dialogMatch = ImageProcessing.FractionalMatch(dialogTitle, DialogTitle);
            return dialogMatch > 0.005;
        }

        /// <summary>
        /// Determines if the dialog box is showing the "Repeat last task?" title
        /// </summary>
        /// <returns>true if the last task dialog is showing</returns>
        protected bool RepeatLastTaskDialog()
        {
            long titleHash = DialogTitleHash();
            return Numerical.CloseEnough(4107517, titleHash, _titleHashPrecision);
        }

        /// <summary>
        /// Determines if the dialog box is showing the "Demon Butler" title
        /// </summary>
        /// <returns>true if the salary dialog is showing</returns>
        protected bool DemonButlerDialog()
        {
            long titleHash = DialogTitleHash();
            return Numerical.CloseEnough(4193761, titleHash, _titleHashPrecision);
        }

        /// <summary>
        /// Determines if the dialog box is showing the "Select an Option" title
        /// </summary>
        /// <returns>true if the select an option dialog is showing</returns>
        protected bool SelectAnOptionDialog()
        {
            long titleHash = DialogTitleHash();
            return Numerical.CloseEnough(4114072, titleHash, _titleHashPrecision);
        }

        /// <summary>
        /// Determines if the dialog box is showing the "What is thy bidding" text or "I am at thy command"
        /// </summary>
        /// <returns>true if one of the waiting for command dialogs is showing</returns>
        protected bool WaitingForCommand()
        {
            int bodyTextCount = DialogBodyText();
            bool maleTextMatch = Numerical.CloseEnough(474, bodyTextCount, 0.01) || Numerical.CloseEnough(439, bodyTextCount, 0.01);    //click or call servant respectively
            bool femaleTextMatch = Numerical.CloseEnough(493, bodyTextCount, 0.01) || Numerical.CloseEnough(458, bodyTextCount, 0.01);
            return maleTextMatch || femaleTextMatch;
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
