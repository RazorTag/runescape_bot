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
    public class LetterReader
    {
        #region properties

        /// <summary>
        /// The empty head node of the letter trie.
        /// </summary>
        private TrieNode TrieHead;

        /// <summary>
        /// Singleton instance of LetterReader.
        /// </summary>
        private static LetterReader instance;

        #endregion

        /// <summary>
        /// Creates the letter reader and sets up the letter trie.
        /// </summary>
        private LetterReader()
        {
            CreateLetterTrie();
        }

        /// <summary>
        /// Gets the singleton instance of LetterReader.
        /// </summary>
        /// <returns></returns>
        public static LetterReader GetInstance()
        {
            if (instance == null)
                instance = new LetterReader();

            return instance;
        }

        #region methods

        /// <summary>
        /// Identifies the letter by comparing pixel columns to the letter trie.
        /// </summary>
        /// <param name="pixelColumns"></param>
        /// <returns>The identified letter. Empty string if letter is unknown.</returns>
        public string ReadLetter(List<int> pixelColumns)
        {
            TrieNode node = TrieHead;
            for (int i = 0; i < pixelColumns.Count; i++)
            {
                if (!node.NextColumn(ref node, pixelColumns[i]))
                {
                    return "";
                }
            }

            return node.Letter;
        }

        /// <summary>
        /// Creates a Trie that maps pixel columns to letters.
        /// </summary>
        private void CreateLetterTrie()
        {
            TrieHead = new TrieNode();
            List<Letter> letters = new List<Letter>();
            Letters.AddAllCharacters(letters);
            
            foreach (Letter letter in letters)
            {
                AddLetterToTrie(letter);
            }
        }

        /// <summary>
        /// Adds a single Letter to the letter trie.
        /// </summary>
        /// <param name="letter">The letter to add</param>
        private void AddLetterToTrie(Letter letter)
        {
            TrieNode node = TrieHead;

            for (int i = 0; i < letter.Bitmap.Length; i++)
            {
                node = node.NextColumn(letter.Bitmap[i]);
            }
            node.MarkEndOfLetter(letter.Value);
        }

        #endregion

        #region TrieNode

        /// <summary>
        /// Node class for the trie used to identify individual letters in text chat.
        /// </summary>
        private class TrieNode
        {
            /// <summary>
            /// Unique key for this pixel column.
            /// </summary>
            public string Letter;

            /// <summary>
            /// Collection of nodes reachable through this node.
            /// </summary>
            private Dictionary<int, TrieNode> NextLetters;

            /// <summary>
            /// Creates a trie node that does not mark the end of a letter.
            /// </summary>
            public TrieNode()
            {
                NextLetters = new Dictionary<int, TrieNode>();
                Letter = "";
            }

            /// <summary>
            /// Marks this node as the end of the specified letter.
            /// </summary>
            /// <param name="letter">The letter that ends here.</param>
            public void MarkEndOfLetter(string letter)
            {
                if (!string.IsNullOrEmpty(Letter))
                    throw new Exception("Duplicate letters " + Letter + " and " + letter);
                Letter = letter;
            }

            /// <summary>
            /// Tries to get the node for the next pixel column.
            /// </summary>
            /// <param name="node">the current pixel column node</param>
            /// <param name="pixelColumn">the next pixel column</param>
            /// <returns>True if a node exists for the next pixel column</returns>
            public bool NextColumn(ref TrieNode node, int pixelColumn)
            {
                TrieNode nextNode;
                bool nextNodeExists;

                if (nextNodeExists = NextLetters.TryGetValue(pixelColumn, out nextNode))
                    node = nextNode;

                return nextNodeExists;
            }

            /// <summary>
            /// Gets (creates if missing) the next node corresponding with the next pixel column.
            /// </summary>
            /// <param name="pixelColumn">The next pixel column.</param>
            /// <returns>The next node.</returns>
            public TrieNode NextColumn(int pixelColumn)
            {
                TrieNode next;

                if (!NextLetters.ContainsKey(pixelColumn))
                {
                    next = new TrieNode();
                    NextLetters.Add(pixelColumn, next);
                }
                else
                {
                    NextLetters.TryGetValue(pixelColumn, out next);
                }

                return next;
            }
        }

        #endregion
    }
}
