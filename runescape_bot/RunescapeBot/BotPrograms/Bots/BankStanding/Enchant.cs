using RunescapeBot.BotPrograms.Popups;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RunescapeBot.BotPrograms
{
    public class Enchant : BankStand
    {
        protected int EnchantLevel;
        protected Point CosmicBankSlot;
        protected Point EnchantableBankSlot;

        public Enchant(RunParams runParams, int enchantLevel) : base(runParams)
        {
            EnchantLevel = enchantLevel;
            CosmicBankSlot = new Point(7, 0);
            EnchantableBankSlot = new Point(6, 0);
        }

        protected override bool WithdrawItems(Bank bank)
        {
            bank.DepositInventory();
            bank.WithdrawAll(CosmicBankSlot.X, CosmicBankSlot.Y);
            bank.WithdrawAll(EnchantableBankSlot.X, EnchantableBankSlot.Y);
            return true;
        }

        protected override bool ProcessInventory()
        {
            Stopwatch watch = new Stopwatch();
            for (int i = 1; i < Inventory.INVENTORY_CAPACITY; i++)
            {
                Point inventorySlot = Inventory.InventoryIndexToCoordinates(i);
                if (!Inventory.Enchant(inventorySlot.X, inventorySlot.Y, EnchantLevel, false, false))
                {
                    return false;
                }
                SafeWaitPlus(1200, 100);
                RunParams.Iterations--;
                if (RunParams.Iterations <= 0 || StopFlag)
                {
                    return false;
                }
            }
            return true;
        }
    }
}
