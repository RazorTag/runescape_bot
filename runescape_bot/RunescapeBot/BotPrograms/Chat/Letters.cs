using System.Collections.Generic;

namespace RunescapeBot.BotPrograms.Chat
{
    internal static class Letters
    {
        #region characters

        //Capital letters
        internal static readonly int[] A = { 0b_0011_1111_1100, 0b_0100_1000_0000, 0b_1000_1000_0000, 0b_1000_1000_0000, 0b_0100_1000_0000, 0b_0011_1111_1100 };
        internal static readonly int[] B = { 0b_1111_1111_1100, 0b_1000_1000_0100, 0b_1000_1000_0100, 0b_1000_1000_0100, 0b_0111_0111_1000 };
        internal static readonly int[] C = { 0b_0011_1111_0000, 0b_0100_0000_1000, 0b_1000_0000_0100, 0b_1000_0000_0100, 0b_0100_0000_1000 };
        internal static readonly int[] D = { 0b_1111_1111_1100, 0b_1000_0000_0100, 0b_1000_0000_0100, 0b_0100_0000_0100, 0b_0011_1111_1000 };
        internal static readonly int[] E = { 0b_1111_1111_1100, 0b_1000_1000_0100, 0b_1000_1000_0100, 0b_1000_0000_0100, 0b_1000_0000_0100 };
        internal static readonly int[] F = { 0b_1111_1111_1100, 0b_1000_1000_0000, 0b_1000_1000_0000, 0b_1000_0000_0000, 0b_1000_0000_0000 };
        internal static readonly int[] G = { 0b_0011_1111_0000, 0b_0100_0000_1000, 0b_1000_0000_0100, 0b_1000_1000_0100, 0b_1000_1000_0100, 0b_0100_0111_1000 };
        internal static readonly int[] H = { 0b_1111_1111_1100, 0b_0000_1000_0000, 0b_0000_1000_0000, 0b_0000_1000_0000, 0b_1111_1111_1100 };
        internal static readonly int[] I = { 0b_1000_0000_0100, 0b_1111_1111_1100, 0b_1000_0000_0100 };
        internal static readonly int[] J = { 0b_1000_0001_1000, 0b_1000_0000_0100, 0b_1000_0000_0100, 0b_1111_1111_1000, 0b_1000_0000_0000, 0b_1000_0000_0000 };
        internal static readonly int[] K = { 0b_1111_1111_1100, 0b_0001_0100_0000, 0b_0010_0010_0000, 0b_0100_0001_0000, 0b_1000_0000_1000, 0b_0000_0000_0100 };
        internal static readonly int[] L = { 0b_1111_1111_1100, 0b_0000_0000_0100, 0b_0000_0000_0100, 0b_0000_0000_0100, 0b_0000_0000_0100 };
        internal static readonly int[] M = { 0b_1111_1111_1100, 0b_0010_0000_0000, 0b_0001_0000_0000, 0b_0000_1000_0000, 0b_0001_0000_0000, 0b_0010_0000_0000, 0b_1111_1111_1100 };
        internal static readonly int[] N = { 0b_1111_1111_1100, 0b_0100_0000_0000, 0b_0011_0000_0000, 0b_0000_1110_0000, 0b_0000_0001_1000, 0b_1111_1111_1100 };
        internal static readonly int[] O = { 0b_0011_1111_0000, 0b_0100_0000_1000, 0b_1000_0000_0100, 0b_1000_0000_0100, 0b_0100_0000_1000, 0b_0011_1111_0000 };
        internal static readonly int[] P = { 0b_1111_1111_1100, 0b_1000_0100_0000, 0b_1000_0100_0000, 0b_0100_1000_0000, 0b_0011_0000_0000 };
        internal static readonly int[] Q = { 0b_0011_1111_0000, 0b_0100_0000_1000, 0b_1000_0010_0100, 0b_1000_0001_0100, 0b_0100_0000_1000, 0b_0011_1111_0100 };
        internal static readonly int[] R = { 0b_1111_1111_1100, 0b_1000_0100_0000, 0b_1000_0110_0000, 0b_1000_1001_0000, 0b_0111_0000_1100 };
        internal static readonly int[] S = { 0b_0010_0000_1000, 0b_0101_0000_0100, 0b_1000_1000_0100, 0b_1000_0100_1000, 0b_0100_0011_0000 };
        internal static readonly int[] T = { 0b_1000_0000_0000, 0b_1000_0000_0000, 0b_1111_1111_1100, 0b_1000_0000_0000, 0b_1000_0000_0000 };
        internal static readonly int[] U = { 0b_1111_1111_0000, 0b_0000_0000_1000, 0b_0000_0000_0100, 0b_0000_0000_0100, 0b_0000_0000_1000, 0b_1111_1111_1100 };
        internal static readonly int[] V = { 0b_1111_1000_0000, 0b_0000_0111_0000, 0b_0000_0000_1100, 0b_0000_0111_0000, 0b_1111_1000_0000 };
        internal static readonly int[] W = { 0b_1111_1111_0000, 0b_0000_0000_1000, 0b_0000_0000_0100, 0b_0000_1111_1100, 0b_0000_0000_0100, 0b_0000_0000_1000, 0b_1111_1111_0000 };
        internal static readonly int[] X = { 0b_1100_0001_1100, 0b_0011_0110_0000, 0b_0000_1000_0000, 0b_0011_0110_0000, 0b_1100_0001_1100 };
        internal static readonly int[] Y = { 0b_1110_0000_0000, 0b_0001_0000_0000, 0b_0000_1111_1100, 0b_0001_0000_0000, 0b_1110_0000_0000 };
        internal static readonly int[] Z = { 0b_1000_0001_1100, 0b_1000_0110_0100, 0b_1000_1000_0100, 0b_1011_0000_0100, 0b_1100_0000_0100 };

        //Lowercase letters
        internal static readonly int[] a = { 0b_0000_0101_1000, 0b_0000_1010_0100, 0b_0000_1010_0100, 0b_0000_1010_0100, 0b_0000_0111_1100 };
        internal static readonly int[] b = { 0b_1111_1111_1100, 0b_0000_1000_0100, 0b_0000_1000_0100, 0b_0000_0100_0100, 0b_0000_0011_1000 };
        internal static readonly int[] c = { 0b_0000_0111_1000, 0b_0000_1000_0100, 0b_0000_1000_0100, 0b_0000_1000_0100 };
        internal static readonly int[] d = { 0b_0000_0011_1000, 0b_0000_0100_0100, 0b_0000_1000_0100, 0b_0000_1000_0100, 0b_1111_1111_1100 };
        internal static readonly int[] e = { 0b_0000_0111_1000, 0b_0000_1001_0100, 0b_0000_1001_0100, 0b_0000_1001_0100, 0b_0000_0110_0100 };
        internal static readonly int[] f = { 0b_0000_0010_0000, 0b_0001_1111_1111, 0b_0010_0010_0000, 0b_0010_0000_0000, 0b_0001_0000_0000 };
        internal static readonly int[] g = { 0b_0000_0111_1000, 0b_0000_1000_0100, 0b_0000_1000_0100, 0b_0000_1000_0100, 0b_0000_0111_1111 };
        internal static readonly int[] h = { 0b_1111_1111_1100, 0b_0000_0100_0000, 0b_0000_1000_0000, 0b_0000_1000_0000, 0b_0000_0111_1100 };
        internal static readonly int[] i = { 0b_0001_0111_1100 };
        internal static readonly int[] j = { 0b_0001_0111_1111 };    //Use only the identifying column for j since it has whitespace in the middle when ignoring bottom 2 rows.
        internal static readonly int[] k = { 0b_1111_1111_1100, 0b_0000_0010_0000, 0b_0000_0101_0000, 0b_0000_1000_1000, 0b_0000_0000_0100 };
        internal static readonly int[] l = { 0b_1111_1111_1100 };
        internal static readonly int[] m = { 0b_0000_0111_1100, 0b_0000_1000_0000, 0b_0000_1000_0000, 0b_0000_0111_1100, 0b_0000_1000_0000, 0b_0000_1000_0000, 0b_0000_0111_1100 };
        internal static readonly int[] n = { 0b_0000_1111_1100, 0b_0000_0100_0000, 0b_0000_1000_0000, 0b_0000_1000_0000, 0b_0000_0111_1100 };
        internal static readonly int[] o = { 0b_0000_0111_1000, 0b_0000_1000_0100, 0b_0000_1000_0100, 0b_0000_1000_0100, 0b_0000_0111_1000 };
        internal static readonly int[] p = { 0b_0000_0111_1111, 0b_0000_1000_0100, 0b_0000_1000_0100, 0b_0000_1000_0100, 0b_0000_0111_1000 };
        internal static readonly int[] q = { 0b_0000_0011_0000, 0b_0000_0100_1000, 0b_0000_1000_0100, 0b_0000_1000_0100, 0b_0000_0111_1111 };
        internal static readonly int[] r = { 0b_0000_1111_1100, 0b_0000_0100_0000, 0b_0000_1000_0000 };
        internal static readonly int[] s = { 0b_0000_0100_0100, 0b_0000_1010_0100, 0b_0000_1010_0100, 0b_0000_1010_0100, 0b_0000_1001_1000 };
        internal static readonly int[] t = { 0b_0111_1111_1000, 0b_0000_1000_0100, 0b_0000_1000_0100 };
        internal static readonly int[] u = { 0b_0000_1111_1000, 0b_0000_0000_0100, 0b_0000_0000_0100, 0b_0000_0000_0100, 0b_0000_1111_1000 };
        internal static readonly int[] v = { 0b_0000_1100_0000, 0b_0000_0011_0000, 0b_0000_0000_1100, 0b_0000_0011_0000, 0b_0000_1100_0000 };
        internal static readonly int[] w = { 0b_0000_1111_1000, 0b_0000_0000_0100, 0b_0000_0011_1100, 0b_0000_0000_0100, 0b_0000_1111_1000 };
        internal static readonly int[] x = { 0b_0000_1000_0100, 0b_0000_0101_1000, 0b_0000_0010_0000, 0b_0000_0101_1000, 0b_0000_1000_0100 };
        internal static readonly int[] y = { 0b_0000_1111_0000, 0b_0000_0000_1000, 0b_0000_0000_1000, 0b_0000_0000_1000, 0b_0000_1111_1111 };
        internal static readonly int[] z = { 0b_0000_1000_1100, 0b_0000_1001_0100, 0b_0000_1010_0100, 0b_0000_1100_0100, 0b_0000_1000_0100 };

        //Digits
        internal static readonly int[] zero = { 0b_0011_1111_0000, 0b_0100_0000_1000, 0b_1000_0111_0100, 0b_1011_1000_0100, 0b_0100_0000_1000, 0b_0011_1111_0000 }; //0
        internal static readonly int[] one = { 0b_0010_0000_0100, 0b_0100_0000_0100, 0b_1111_1111_1100, 0b_0000_0000_0100, 0b_0000_0000_0100 }; //1
        internal static readonly int[] two = { 0b_0100_0000_1100, 0b_1000_0001_0100, 0b_1000_0010_0100, 0b_1000_0100_0100, 0b_0100_1000_0100, 0b_0011_0000_0100 }; //2
        internal static readonly int[] three = { 0b_0100_0000_1000, 0b_1000_1000_0100, 0b_1000_1000_0100, 0b_1000_1000_0100, 0b_0111_0111_1000 }; //3
        internal static readonly int[] four = { 0b_1111_1111_0000, 0b_0000_0001_0000, 0b_0000_0001_0000, 0b_0000_1111_1100, 0b_0000_0001_0000, 0b_0000_0001_0000 }; //4
        internal static readonly int[] five = { 0b_1111_0000_1000, 0b_1001_0000_0100, 0b_1001_0000_0100, 0b_1000_1000_0100, 0b_1000_0111_1000 }; //5
        internal static readonly int[] six = { 0b_0011_1111_0000, 0b_0100_0100_1000, 0b_1000_1000_0100, 0b_1000_1000_0100, 0b_0100_0100_1000, 0b_0000_0011_0000 }; //6
        internal static readonly int[] seven = { 0b_1000_0000_1100, 0b_1000_0011_0000, 0b_1000_1100_0000, 0b_1011_0000_0000, 0b_1100_0000_0000 }; //7
        internal static readonly int[] eight = { 0b_0010_0011_0000, 0b_0101_0100_1000, 0b_1000_1000_0100, 0b_1000_1000_0100, 0b_0101_0100_1000, 0b_0010_0011_0000 }; //8
        internal static readonly int[] nine = { 0b_0111_0000_0000, 0b_1000_1000_0000, 0b_1000_0100_0000, 0b_1000_0100_0000, 0b_1000_0100_0000, 0b_0111_1111_1100 }; //9

        //Punctuation & Symbols
        internal static readonly int[] openSingleQuote = { 0b_1000_0000_0000, 0b_0100_0000_0000 }; //`
        internal static readonly int[] tilde = { 0b_0000_0100_0000, 0b_0000_1000_0000, 0b_0001_0000_0000, 0b_0001_0000_0000, 0b_0001_0000_0000, 0b_0000_1000_0000, 0b_0000_0100_0000, 0b_0000_0100_0000, 0b_0000_1000_0000 }; //~
        internal static readonly int[] exclamation = { 0b_1111_1110_1100 }; //!
        internal static readonly int[] at = { 0b_0001_1111_0000, 0b_0010_0000_1000, 0b_0100_1110_0100, 0b_1001_0001_0100, 0b_1010_0001_0100, 0b_1010_0001_0100, 0b_1010_0010_0100, 0b_1001_1100_0100, 0b_1000_0010_0100, 0b_0100_0001_0100, 0b_0011_1110_0000 }; //@
        internal static readonly int[] pound = { 0b_0001_0010_0000, 0b_0001_0010_1100, 0b_0001_0011_0000, 0b_0001_1110_0000, 0b_0011_0010_0000, 0b_1101_0010_1100, 0b_0001_0011_0000, 0b_0001_1110_0000, 0b_0011_0010_0000, 0b_1101_0010_0000, 0b_0001_0010_0000 }; //#
        internal static readonly int[] dollar = { 0b_0010_0000_1000, 0b_0101_0000_0100, 0b_1000_1000_0100, 0b_1111_1111_1110, 0b_1000_0100_1000, 0b_0100_0011_0000 }; //$
        internal static readonly int[] percent = { 0b_0111_0000_0000, 0b_1000_1000_1000, 0b_1000_1001_0000, 0b_0111_0010_0000, 0b_0000_0100_0000, 0b_0000_1011_1000, 0b_0001_0100_0100, 0b_0010_0100_0100, 0b_0100_0011_1000 }; //%
        internal static readonly int[] caret = { 0b_0000_1000_0000, 0b_0111_0000_0000, 0b_1000_0000_0000, 0b_1000_0000_0000, 0b_0111_0000_0000, 0b_0000_1000_0000 }; //^
        internal static readonly int[] ampersand = { 0b_0000_0011_0000, 0b_0110_0100_1000, 0b_1001_1000_0100, 0b_1000_1000_0100, 0b_1000_1000_0100, 0b_0111_0100_1000, 0b_0000_0011_0000, 0b_0000_0011_0000, 0b_0000_0100_1000, 0b_0000_0000_0100 }; //&
        internal static readonly int[] asterisk = { 0b_0010_0000_0000, 0b_0010_0100_0000, 0b_0010_1000_0000, 0b_1111_0000_0000, 0b_0010_1000_0000, 0b_0010_0100_0000, 0b_0010_0000_0000 }; //*
        internal static readonly int[] openParenthesis = { 0b_0011_1111_1000, 0b_0100_0000_0100, 0b_1000_0000_0010 }; //(
        internal static readonly int[] closeParenthesis = { 0b_1000_0000_0010, 0b_0100_0000_0100, 0b_0011_1111_1000 }; //)
        internal static readonly int[] underscore = { 0b_0000_0000_0100, 0b_0000_0000_0100, 0b_0000_0000_0100, 0b_0000_0000_0100, 0b_0000_0000_0100, 0b_0000_0000_0100, 0b_0000_0000_0100 }; //_
        internal static readonly int[] minus = { 0b_0000_0100_0000, 0b_0000_0100_0000, 0b_0000_0100_0000, 0b_0000_0100_0000 }; //-
        internal static readonly int[] plus = { 0b_0000_0100_0000, 0b_0000_0100_0000, 0b_0000_0100_0000, 0b_0011_1111_1000, 0b_0000_0100_0000, 0b_0000_0100_0000, 0b_0000_0100_0000 }; //+
        internal static readonly int[] equal = { 0b_0000_1001_0000, 0b_0000_1001_0000, 0b_0000_1001_0000, 0b_0000_1001_0000, 0b_0000_1001_0000, 0b_0000_1001_0000 }; //=
        internal static readonly int[] openCurlyBracket = { 0b_0000_0100_0000, 0b_0000_1010_0000, 0b_0111_0001_1100, 0b_1000_0000_0010, 0b_1000_0000_0010 }; //{
        internal static readonly int[] closeCurlyBracket = { 0b_1000_0000_0010, 0b_1000_0000_0010, 0b_0111_0001_1100, 0b_0000_1010_0000, 0b_0000_0100_0000 }; //}
        internal static readonly int[] openSquareBracket = { 0b_1111_1111_1110, 0b_1000_0000_0010, 0b_1000_0000_0010, 0b_1000_0000_0010 }; //[
        internal static readonly int[] closeSquareBracket = { 0b_1000_0000_0010, 0b_1000_0000_0010, 0b_1000_0000_0010, 0b_1111_1111_1110 }; //]
        internal static readonly int[] colon = { 0b_0000_1000_0100 }; //:
        internal static readonly int[] semicolon = { 0b_0000_0000_0100, 0b_0000_0010_1000, 0b_0010_0011_0000 }; //;
        internal static readonly int[] apostrophe = { 0b_0100_0000_0000, 0b_1000_0000_0000 }; //'
        //internal static readonly int[] doubleQuote = { 0b_1100_0000_0000, 0b_0000_0000_0000, 0b_1100_0000_0000 }; //" cannot be distinguished from two single quotes
        internal static readonly int[] openAngleBracket = { 0b_0000_1000_0000, 0b_0001_0100_0000, 0b_0010_0010_0000, 0b_0010_0010_0000, 0b_0010_0010_0000, 0b_0100_0001_0000 }; //<
        internal static readonly int[] closeAngleBracket = { 0b_0100_0001_0000, 0b_0010_0010_0000, 0b_0010_0010_0000, 0b_0010_0010_0000, 0b_0001_0100_0000, 0b_0000_1000_0000 }; //>
        internal static readonly int[] comma = { 0b_0000_0001_0010, 0b_0000_0001_1100 }; //,
        internal static readonly int[] period = { 0b_0000_0000_0100 }; //.
        internal static readonly int[] question = { 0b_0000_0111_0100, 0b_0000_1000_0000, 0b_0001_0000_0000, 0b_1110_0000_0000 }; //?   //ignore the left hook since it is disconnected
        internal static readonly int[] forwardSlash = { 0b_0000_0001_1100, 0b_0000_1110_0000, 0b_0111_0000_0000, 0b_1000_0000_0000 }; //"/"
        internal static readonly int[] backslash = { 0b_1000_0000_0000, 0b_0111_0000_0000, 0b_0000_1110_0000, 0b_0000_0001_1100 }; //"\"
        //internal static readonly int[] pipe = { 0b_1111_1111_1100 }; //|   cannot be used since it matches a lowercase l

        //Combination letters
        internal static readonly int[] openCloseParenthesis = ConcatenateLetters(openParenthesis, closeParenthesis); //()
        internal static readonly int[] openCloseCurlyBrackets = ConcatenateLetters(openCurlyBracket, closeCurlyBracket); //{}
        internal static readonly int[] openCloseSquareBrackets = ConcatenateLetters(openSquareBracket, closeSquareBracket); //[]
        internal static readonly int[] openCloseAngleBrackets = ConcatenateLetters(openAngleBracket, closeAngleBracket); //<>

        #endregion

        #region accumulators

        /// <summary>
        /// Adds all characters to a list of Character.
        /// </summary>
        /// <param name="charList">A list to store Character.</param>
        internal static void AddAllCharacters(List<Letter> letterList)
        {
            AddLetters(letterList);
            AddDigits(letterList);
            AddSymbols(letterList);
        }

        /// <summary>
        /// Adds all letters to a list of characters.
        /// </summary>
        /// <param name="charList">A list to store characters.</param>
        internal static void AddLetters(List<Letter> letterList)
        {
            AddUpperLetters(letterList);
            AddLowerLetters(letterList);
        }

        /// <summary>
        /// Adds uppercase letters to a list of characters.
        /// </summary>
        /// <param name="charList">A list to store characters.</param>
        internal static void AddUpperLetters(List<Letter> letterList)
        {
            AddLetter(letterList, A, "A");
            AddLetter(letterList, B, "B");
            AddLetter(letterList, C, "C");
            AddLetter(letterList, D, "D");
            AddLetter(letterList, E, "E");
            AddLetter(letterList, F, "F");
            AddLetter(letterList, G, "G");
            AddLetter(letterList, H, "H");
            AddLetter(letterList, I, "I");
            AddLetter(letterList, J, "J");
            AddLetter(letterList, K, "K");
            AddLetter(letterList, L, "L");
            AddLetter(letterList, M, "M");
            AddLetter(letterList, N, "N");
            AddLetter(letterList, O, "O");
            AddLetter(letterList, P, "P");
            AddLetter(letterList, Q, "Q");
            AddLetter(letterList, R, "R");
            AddLetter(letterList, S, "S");
            AddLetter(letterList, T, "T");
            AddLetter(letterList, U, "U");
            AddLetter(letterList, V, "V");
            AddLetter(letterList, W, "W");
            AddLetter(letterList, X, "X");
            AddLetter(letterList, Y, "Y");
            AddLetter(letterList, Z, "Z");
        }

        /// <summary>
        /// Adds lowercase letters to a list of characters.
        /// </summary>
        /// <param name="charList">A list to store characters.</param>
        internal static void AddLowerLetters(List<Letter> letterList)
        {
            AddLetter(letterList, a, "a");
            AddLetter(letterList, b, "b");
            AddLetter(letterList, c, "c");
            AddLetter(letterList, d, "d");
            AddLetter(letterList, e, "e");
            AddLetter(letterList, f, "f");
            AddLetter(letterList, g, "g");
            AddLetter(letterList, h, "h");
            AddLetter(letterList, i, "i");
            AddLetter(letterList, j, "j");
            AddLetter(letterList, k, "k");
            AddLetter(letterList, l, "l");
            AddLetter(letterList, m, "m");
            AddLetter(letterList, n, "n");
            AddLetter(letterList, o, "o");
            AddLetter(letterList, p, "p");
            AddLetter(letterList, q, "q");
            AddLetter(letterList, r, "r");
            AddLetter(letterList, s, "s");
            AddLetter(letterList, t, "t");
            AddLetter(letterList, u, "u");
            AddLetter(letterList, v, "v");
            AddLetter(letterList, w, "w");
            AddLetter(letterList, x, "x");
            AddLetter(letterList, y, "y");
            AddLetter(letterList, z, "z");
        }

        /// <summary>
        /// Adds digits to the list of characters.
        /// </summary>
        /// <param name="letterList">list of characters</param>
        internal static void AddDigits(List<Letter> letterList)
        {
            AddLetter(letterList, zero, "0");
            AddLetter(letterList, one, "1");
            AddLetter(letterList, two, "2");
            AddLetter(letterList, three, "3");
            AddLetter(letterList, four, "4");
            AddLetter(letterList, five, "5");
            AddLetter(letterList, six, "6");
            AddLetter(letterList, seven, "7");
            AddLetter(letterList, eight, "8");
            AddLetter(letterList, nine, "9");
        }

        /// <summary>
        /// Adds punctuation and symbols to a list of characters.
        /// </summary>
        /// <param name="charList">A list to store characters.</param>
        internal static void AddSymbols(List<Letter> letterList)
        {
            AddLetter(letterList, openSingleQuote, "`");
            AddLetter(letterList, tilde, "~");
            AddLetter(letterList, exclamation, "!");
            AddLetter(letterList, at, "@");
            AddLetter(letterList, pound, "#");
            AddLetter(letterList, dollar, "$");
            AddLetter(letterList, percent, "%");
            AddLetter(letterList, caret, "^");
            AddLetter(letterList, ampersand, "&");
            AddLetter(letterList, asterisk, "*");
            AddLetter(letterList, openParenthesis, "(");
            AddLetter(letterList, closeParenthesis, ")");
            AddLetter(letterList, underscore, "_");
            AddLetter(letterList, minus, "-");
            AddLetter(letterList, plus, "+");
            AddLetter(letterList, equal, "=");
            AddLetter(letterList, openCurlyBracket, "{");
            AddLetter(letterList, closeCurlyBracket, "}");
            AddLetter(letterList, openSquareBracket, "[");
            AddLetter(letterList, closeSquareBracket, "]");
            AddLetter(letterList, colon, ":");
            AddLetter(letterList, semicolon, ";");
            AddLetter(letterList, apostrophe, "\'");
            //AddLetter(letterList, doubleQuote, "\"");
            AddLetter(letterList, openAngleBracket, "<");
            AddLetter(letterList, closeAngleBracket, ">");
            AddLetter(letterList, comma, ",");
            AddLetter(letterList, period, ".");
            AddLetter(letterList, question, "?");
            AddLetter(letterList, forwardSlash, "/");
            AddLetter(letterList, backslash, "\\");
            //AddLetter(letterList, pipe, "|");

            AddLetter(letterList, openCloseParenthesis, "()");
            AddLetter(letterList, openCloseCurlyBrackets, "{}");
            AddLetter(letterList, openCloseSquareBrackets, "[]");
            AddLetter(letterList, openCloseAngleBrackets, "<>");
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

        /// <summary>
        /// Concatenates the pixel columns for multiple letters.
        /// </summary>
        /// <param name="letters">List of pixel column arrays for multiple letters.</param>
        /// <returns>Concatenated pixel column arrays.</returns>
        private static int[] ConcatenateLetters(params int[][] letters)
        {
            var combinedLetter = new List<int>();
            for (int letter = 0; letter < letters.Length; letter++)
            {
                for (int column = 0; column < letters[letter].Length; column++)
                {
                    combinedLetter.Add(letters[letter][column]);
                }
            }
            return combinedLetter.ToArray();
        }

        #endregion
    }
}
