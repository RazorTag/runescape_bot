using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RunescapeBot.BotPrograms.Chat
{
    public class Letter
    {
        /// <summary>
        /// char value for this letter
        /// </summary>
        public string Value;

        /// <summary>
        /// Binary bitmap for this letter
        /// </summary>
        public int[] Bitmap;

        /// <summary>
        /// Gets a value from the bitmap.
        /// </summary>
        /// <param name="i">bitmap index</param>
        /// <returns>The specified value from the bitmap.</returns>
        public int this[int i]
        {
            get { return Bitmap[i]; }
        }

        /// <summary>
        /// Creates a Letter
        /// </summary>
        /// <param name="bitmap">array binary column values for this letter (left to right)</param>
        /// <param name="letter">char value of this letter</param>
        public Letter(int[] bitmap, string letter)
        {
            Bitmap = bitmap;
            Value = letter;
        }

        #region methods

        /// <summary>
        /// Calcuates the hash value of a column of pixels taken from a letter.
        /// </summary>
        /// <param name="pixelColumn">the column of pixels to hash</param>
        /// <returns>A hash value for a column of pixels.</returns>
        public static int ColumnValue(bool[] pixelColumn)
        {
            int pow = 1;
            int value = 0;

            for (int i = pixelColumn.Length-1; i >= 0; i--)
            {
                if (pixelColumn[i])
                    value += pow;
                pow *= 2;
            }

            return value;
        }

        #endregion
    }
}
