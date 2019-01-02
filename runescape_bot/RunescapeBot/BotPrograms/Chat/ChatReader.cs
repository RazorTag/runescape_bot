using RunescapeBot.ImageTools;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RunescapeBot.BotPrograms.Chat
{
    /// <summary>
    /// Translates game text into its equivalent string value.
    /// </summary>
    public class ChatReader
    {
        /// <summary>
        /// The number of pixel rows to look at when reading a chat row.
        /// Starts from the top pixel row of a chat row.
        /// We ignore the last 2 of 14 rows because apostrophes can overlap those.
        /// </summary>
        private const int ROWS_TO_READ = TextBoxTool.ROW_HEIGHT - 2;

        public ChatReader()
        {
            CreateTrie();
        }

        private void CreateTrie()
        {
            //TODO
        }

        public string ReadChatRow(bool[,] chatRow)
        {
            //TODO
            return "";
        }


        private class TrieNode
        {
            /// <summary>
            /// Unique key for this pixel column
            /// </summary>
            public int Key;

            public TrieNode(bool[] column)
            {

            }

            /// <summary>
            /// Creates a key for a column of pixels.
            /// </summary>
            /// <param name="column"></param>
            /// <returns>A key for the given array. Guaranteed unique hash among different arrays of the same length.</returns>
            private int Hash(bool[] column)
            {
                int hash = 0;
                int power = 1;

                for (int row = 0; row < Math.Min(ROWS_TO_READ, column.Length); row++)
                {
                    if (column[row])
                        hash += power;

                    power *= 2;
                }

                return hash;
            }
        }
    }
}
