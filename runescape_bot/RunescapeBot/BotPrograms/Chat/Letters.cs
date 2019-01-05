﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RunescapeBot.BotPrograms.Chat
{
    public static class Letters
    {
        #region characters

        //Capital letters
        static readonly int[] A = { 0b_0011_1111_1100, 0b_0100_1000_0000, 0b_1000_1000_0000, 0b_1000_1000_0000, 0b_0100_1000_0000, 0b_0011_1111_1100 };
        static readonly int[] B = { 0b_1111_1111_1100, 0b_1000_1000_0100, 0b_1000_1000_0100, 0b_1000_1000_0100, 0b_0111_0111_1000 };
        static readonly int[] C = { 0b_0011_1111_0000, 0b_0100_0000_1000, 0b_1000_0000_0100, 0b_1000_0000_0100, 0b_0100_0000_1000 };
        static readonly int[] D = { 0b_1111_1111_1100, 0b_1000_0000_0100, 0b_1000_0000_0100, 0b_0100_0000_0100, 0b_0011_1111_1000 };
        static readonly int[] E = { 0b_1111_1111_1100, 0b_1000_1000_0100, 0b_1000_1000_0100, 0b_1000_0000_0100, 0b_1000_0000_0100 };
        static readonly int[] F = { 0b_1111_1111_1100, 0b_1000_1000_0000, 0b_1000_1000_0000, 0b_1000_0000_0000, 0b_1000_0000_0000 };
        static readonly int[] G = { 0b_0011_1111_0000, 0b_0100_0000_1000, 0b_1000_0000_0100, 0b_1000_1000_0100, 0b_1000_1000_0100, 0b_0100_0111_1000 };
        static readonly int[] H = { 0b_1111_1111_1100, 0b_0000_1000_0000, 0b_0000_1000_0000, 0b_0000_1000_0000, 0b_1111_1111_1100 };
        static readonly int[] I = { 0b_1000_0000_0100, 0b_1111_1111_1100, 0b_1000_0000_0100 };
        static readonly int[] J = { 0b_1000_0001_1000, 0b_1000_0000_0100, 0b_1000_0000_0100, 0b_1111_1111_1000, 0b_1000_0000_0000, 0b_1000_0000_0000 };
        static readonly int[] K = { 0b_1111_1111_1100, 0b_0001_0100_0000, 0b_0010_0010_0000, 0b_0100_0001_0000, 0b_1000_0000_1000, 0b_0000_0000_0100 };
        static readonly int[] L = { 0b_1111_1111_1100, 0b_0000_0000_0100, 0b_0000_0000_0100, 0b_0000_0000_0100, 0b_0000_0000_0100 };
        static readonly int[] M = { 0b_1111_1111_1100, 0b_0010_0000_0000, 0b_0001_0000_0000, 0b_0000_1000_0000, 0b_0001_0000_0000, 0b_0010_0000_0000, 0b_1111_1111_1100 };
        static readonly int[] N = { 0b_1111_1111_1100, 0b_0100_0000_0000, 0b_0011_0000_0000, 0b_0000_1110_0000, 0b_0000_0001_1000, 0b_1111_1111_1100 };
        static readonly int[] O = { 0b_0011_1111_0000, 0b_0100_0000_1000, 0b_1000_0000_0100, 0b_1000_0000_0100, 0b_0100_0000_1000, 0b_0011_1111_0000 };
        static readonly int[] P = { 0b_1111_1111_1100, 0b_1000_0100_0000, 0b_1000_0100_0000, 0b_0100_1000_0000, 0b_0100_1000_0000, 0b_0011_0000_0000 };
        static readonly int[] Q = { 0b_0011_1111_0000, 0b_0100_0000_1000, 0b_1000_0010_0100, 0b_1000_0001_0100, 0b_0100_0000_1000, 0b_0011_1111_0100 };
        static readonly int[] R = { 0b_1111_1111_1100, 0b_1000_0100_0000, 0b_1000_0110_0000, 0b_1000_1001_0000, 0b_0111_0000_1100 };
        static readonly int[] S = { 0b_0010_0000_1000, 0b_0101_0000_0100, 0b_1000_1000_0100, 0b_1000_0100_1000, 0b_0100_0011_0000 };
        static readonly int[] T = { 0b_1000_0000_0000, 0b_1000_0000_0000, 0b_1111_1111_1100, 0b_1000_0000_0000, 0b_1000_0000_0000 };
        static readonly int[] U = { 0b_1111_1111_0000, 0b_0000_0000_1000, 0b_0000_0000_0100, 0b_0000_0000_0100, 0b_0000_0000_1000, 0b_1111_1111_1100 };
        static readonly int[] V = { 0b_1111_1000_0000, 0b_0000_0111_0000, 0b_0000_0000_1100, 0b_0000_0111_0000, 0b_1111_1000_0000 };
        static readonly int[] W = { 0b_1111_1111_0000, 0b_0000_0000_1000, 0b_0000_0000_0100, 0b_0000_1111_1100, 0b_0000_0000_0100, 0b_0000_0000_1000, 0b_1111_1111_0000 };
        static readonly int[] X = { 0b_1100_0001_1100, 0b_0011_0110_0000, 0b_0000_1000_0000, 0b_0011_0110_0000, 0b_1100_0001_1100 };
        static readonly int[] Y = { 0b_1110_0000_0000, 0b_0001_0000_0000, 0b_0000_1111_1100, 0b_0001_0000_0000, 0b_1110_0000_0000 };
        static readonly int[] Z = { 0b_1000_0001_1100, 0b_1000_0110_0100, 0b_1000_1000_0100, 0b_1011_0000_0100, 0b_1100_0000_0100 };

        //Lowercase letters
        static readonly int[] a = { 0b_0000_0101_1000, 0b_0000_1010_0100, 0b_0000_1010_0100, 0b_0000_1010_0100, 0b_0000_0111_1100 };
        static readonly int[] b = { 0b_1111_1111_1100, 0b_0000_1000_0100, 0b_0000_1000_0100, 0b_0000_0100_0100, 0b_0000_0011_1000 };
        static readonly int[] c = { 0b_0000_0111_1000, 0b_0000_1000_0100, 0b_0000_1000_0100, 0b_0000_1000_0100 };
        static readonly int[] d = { 0b_0000_0011_1000, 0b_0000_0100_0100, 0b_0000_1000_0100, 0b_0000_1000_0100, 0b_1111_1111_1100 };
        static readonly int[] e = { 0b_0000_0111_1000, 0b_0000_1001_0100, 0b_0000_1001_0100, 0b_0000_1001_0100, 0b_0000_0110_0100 };
        static readonly int[] f = { 0b_0000_0010_0000, 0b_0001_1111_1111, 0b_0010_0010_0000, 0b_0010_0000_0000, 0b_0001_0000_0000 };
        static readonly int[] g = { 0b_0000_0111_1000, 0b_0000_1000_0100, 0b_0000_1000_0100, 0b_0000_1000_0100, 0b_0000_0111_1111 };
        static readonly int[] h = { 0b_1111_1111_1100, 0b_0000_0100_0000, 0b_0000_1000_0000, 0b_0000_1000_0000, 0b_0000_0111_1100 };
        static readonly int[] i = { 0b_0001_0111_1100 };
        static readonly int[] j = { 0b_0000_0000_0001, 0b_0000_0000_0000, 0b_0000_0000_0000, 0b_0001_0111_1111 };
        static readonly int[] k = { 0b_1111_1111_1100, 0b_0000_0010_0000, 0b_0000_0101_0000, 0b_0000_1000_1000, 0b_0000_0000_0100 };
        static readonly int[] l = { 0b_1111_1111_1100 };
        static readonly int[] m = { 0b_0000_0111_1100, 0b_0000_1000_0000, 0b_0000_1000_0000, 0b_0000_0111_1100, 0b_0000_1000_0000, 0b_0000_1000_0000, 0b_0000_0111_1100 };
        static readonly int[] n = { 0b_0000_1111_1100, 0b_0000_0100_0000, 0b_0000_1000_0000, 0b_0000_1000_0000, 0b_0000_0111_1100 };
        static readonly int[] o = { 0b_0000_0111_1000, 0b_0000_1000_0100, 0b_0000_1000_0100, 0b_0000_1000_0100, 0b_0000_0111_1000 };
        static readonly int[] p = { 0b_0000_0111_1111, 0b_0000_1000_0100, 0b_0000_1000_0100, 0b_0000_1000_0100, 0b_0000_0111_1000 };
        static readonly int[] q = { 0b_0000_0011_0000, 0b_0000_0100_1000, 0b_0000_1000_0100, 0b_0000_1000_0100, 0b_0000_0111_1111 };
        static readonly int[] r = { 0b_0000_1111_1100, 0b_0000_0100_0000, 0b_0000_1000_0000 };
        static readonly int[] s = { 0b_0000_0100_0100, 0b_0000_1010_0100, 0b_0000_1010_0100, 0b_0000_1010_0100, 0b_0000_1001_1000 };
        static readonly int[] t = { 0b_0111_1111_1000, 0b_0000_1000_0100, 0b_0000_1000_0100 };
        static readonly int[] u = { 0b_0000_1111_1000, 0b_0000_0000_0100, 0b_0000_0000_0100, 0b_0000_0000_0100, 0b_0000_1111_1000 };
        static readonly int[] v = { 0b_0000_1100_0000, 0b_0000_0011_0000, 0b_0000_0000_1100, 0b_0000_0011_0000, 0b_0000_1100_0000 };
        static readonly int[] w = { 0b_0000_1111_1000, 0b_0000_0000_0100, 0b_0000_0011_1100, 0b_0000_0000_0100, 0b_0000_1111_1000 };
        static readonly int[] x = { 0b_0000_1000_0100, 0b_0000_0101_1000, 0b_0000_0010_0000, 0b_0000_0101_1000, 0b_0000_1000_0100 };
        static readonly int[] y = { 0b_0000_1111_0000, 0b_0000_0000_1000, 0b_0000_0000_1000, 0b_0000_0000_1000, 0b_0000_1111_1111 };
        static readonly int[] z = { 0b_0000_1000_1100, 0b_0000_1001_0100, 0b_0000_1010_0100, 0b_0000_1100_0100, 0b_0000_1000_0100 };

        //Punctuation & Symbols
        static readonly int[] openSingleQuote = { 0b_1000_0000_0000, 0b_0100_0000_0000 }; //`
        static readonly int[] tilde = { 0b_0000_0100_0000, 0b_0000_1000_0000, 0b_0001_0000_0000, 0b_0001_0000_0000, 0b_0001_0000_0000, 0b_0000_1000_0000, 0b_0000_0100_0000, 0b_0000_0100_0000, 0b_0000_1000_0000 }; //~
        static readonly int[] exclamation = { 0b_1111_1110_1100 }; //!
        static readonly int[] at = { 0b_0001_1111_0000, 0b_0010_0000_1000, 0b_0100_1110_0100, 0b_1001_0001_0100, 0b_1010_0001_0100, 0b_1010_0001_0100, 0b_1010_0010_0100, 0b_1001_1100_0100, 0b_1000_0010_0100, 0b_0100_0001_0100, 0b_0011_1110_0000 }; //@
        static readonly int[] pound = { 0b_0001_0010_0000, 0b_0001_0010_1100, 0b_0001_0011_0000, 0b_0001_1110_0000, 0b_0011_0010_0000, 0b_1101_0010_1100, 0b_0001_0011_0000, 0b_0001_1110_0000, 0b_0011_0010_0000, 0b_1101_0010_0000, 0b_0001_0010_0000 }; //#
        static readonly int[] dollar = { 0b_0010_0000_1000, 0b_0101_0000_0100, 0b_1000_1000_0100, 0b_1111_1111_1110, 0b_1000_0100_1000, 0b_0100_0011_0000 }; //$
        static readonly int[] percent = { 0b_0111_0000_0000, 0b_1000_1000_1000, 0b_1000_1001_0000, 0b_0111_0010_0000, 0b_0000_0100_0000, 0b_0000_1011_1000, 0b_0001_0100_0100, 0b_0010_0100_0100, 0b_0100_0011_1000 }; //%
        static readonly int[] caret = { 0b_0000_1000_0000, 0b_0111_0000_0000, 0b_1000_0000_0000, 0b_1000_0000_0000, 0b_0111_0000_0000, 0b_0000_1000_0000 }; //^
        static readonly int[] ampersand = { 0b_0000_0011_0000, 0b_0110_0100_1000, 0b_1001_1000_0100, 0b_1000_1000_0100, 0b_1000_1000_0100, 0b_0111_0100_1000, 0b_0000_0011_0000, 0b_0000_0011_0000, 0b_0000_0100_1000, 0b_0000_0000_0100 }; //&
        static readonly int[] asterisk = { 0b_0010_0000_0000, 0b_0010_0100_0000, 0b_0010_1000_0000, 0b_1111_0000_0000, 0b_0010_1000_0000, 0b_0010_0100_0000, 0b_0010_0000_0000 }; //*
        static readonly int[] openParenthesis = { 0b_0011_1111_1000, 0b_0100_0000_0100, 0b_1000_0000_0010 }; //(
        static readonly int[] closeParenthesis = { 0b_1000_0000_0010, 0b_0100_0000_0100, 0b_0011_1111_1000 }; //)
        static readonly int[] underscore = { 0b_0000_0000_0100, 0b_0000_0000_0100, 0b_0000_0000_0100, 0b_0000_0000_0100, 0b_0000_0000_0100, 0b_0000_0000_0100, 0b_0000_0000_0100 }; //_
        static readonly int[] minus = { 0b_0000_0100_0000, 0b_0000_0100_0000, 0b_0000_0100_0000, 0b_0000_0100_0000 }; //-
        static readonly int[] plus = { 0b_0000_0100_0000, 0b_0000_0100_0000, 0b_0000_0100_0000, 0b_0011_1111_1000, 0b_0000_0100_0000, 0b_0000_0100_0000, 0b_0000_0100_0000 }; //+
        static readonly int[] openCurlyBracket = { 0b_0000_0100_0000, 0b_0000_1010_0000, 0b_0111_0001_1100, 0b_1000_0000_0010, 0b_1000_0000_0010 }; //{
        static readonly int[] closeCurlyBracet = { 0b_1000_0000_0010, 0b_1000_0000_0010, 0b_0111_0001_1100, 0b_0000_1010_0000, 0b_0000_0100_0000 }; //}
        static readonly int[] openSquareBracket = { 0b_1111_1111_1110, 0b_1000_0000_0010, 0b_1000_0000_0010, 0b_1000_0000_0010 }; //[
        static readonly int[] closeSquareBracket = { 0b_1000_0000_0010, 0b_1000_0000_0010, 0b_1000_0000_0010, 0b_1111_1111_1110 }; //]
        static readonly int[] colon = { 0b_0000_1000_0100 }; //:
        static readonly int[] semicolon = { 0b_0000_0000_0100, 0b_0000_0010_1000, 0b_0010_0011_0000 }; //;
        static readonly int[] apostrophe = { 0b_0100_0000_0000, 0b_1000_0000_0000 }; //'
        static readonly int[] doubleQuote = { 0b_1100_0000_0000, 0b_1100_0000_0000 }; //"
        static readonly int[] openAngleBracket = { 0b_0000_1000_0000, 0b_0001_0100_0000, 0b_0010_0010_0000, 0b_0010_0010_0000, 0b_0010_0010_0000, 0b_0100_0001_0000 }; //<
        static readonly int[] closeAngleBracket = { 0b_0100_0001_0000, 0b_0010_0010_0000, 0b_0010_0010_0000, 0b_0010_0010_0000, 0b_0001_0100_0000, 0b_0000_1000_0000 }; //>
        static readonly int[] comma = { 0b_0000_0001_0010, 0b_0000_0001_1100 }; //,
        static readonly int[] period = { 0b_0000_0000_0100 }; //.
        static readonly int[] question = { 0b_1000_0000_0000, 0b_0000_0000_0000, 0b_0000_0000_0000, 0b_0000_0111_0100, 0b_0000_1000_0000, 0b_0001_0000_0000, 0b_1110_0000_0000 }; //?
        static readonly int[] forwardSlash = { 0b_0000_0001_1100, 0b_0000_1110_0000, 0b_0111_0000_0000, 0b_1000_0000_0000 }; //"/"
        static readonly int[] backslash = { 0b_1000_0000_0000, 0b_0111_0000_0000, 0b_0000_1110_0000, 0b_0000_0001_1100 }; //"\"
        static readonly int[] pipe = { 0b_1111_1111_1100 }; //|

        #endregion

        #region accumulators

        /// <summary>
        /// Adds all characters to a list of Character.
        /// </summary>
        /// <param name="charList">A list to store Character.</param>
        public static void AddAllCharacters(List<Letter> letterList)
        {
            AddLetters(letterList);
            AddSymbols(letterList);
        }

        /// <summary>
        /// Adds all letters to a list of Character.
        /// </summary>
        /// <param name="charList">A list to store Character.</param>
        public static void AddLetters(List<Letter> letterList)
        {
            AddUpperLetters(letterList);
            AddLowerLetters(letterList);
        }

        /// <summary>
        /// Adds uppercase letters to a list of Character.
        /// </summary>
        /// <param name="charList">A list to store Character.</param>
        public static void AddUpperLetters(List<Letter> letterList)
        {
            AddLetter(letterList, A, "A");
            //TODO
        }

        /// <summary>
        /// Adds lowercase letters to a list of Character.
        /// </summary>
        /// <param name="charList">A list to store Character.</param>
        public static void AddLowerLetters(List<Letter> letterList)
        {
            //TODO
        }

        /// <summary>
        /// Adds punctuation and symbols to a list of Character.
        /// </summary>
        /// <param name="charList">A list to store Character.</param>
        public static void AddSymbols(List<Letter> letterList)
        {
            //TODO
        }

        /// <summary>
        /// Adds a Letter to a list.
        /// </summary>
        /// <param name="letterList">the list to add to</param>
        /// <param name="bitmap">column values for the letter</param>
        /// <param name="value">the letter's string value</param>
        private static void AddLetter(List<Letter> letterList, int[] bitmap, string value)
        {
            letterList.Add(new Letter(bitmap, value));
        }

        #endregion
    }
}