using RunescapeBot.Common;
using RunescapeBot.ImageTools;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static RunescapeBot.ImageTools.RGBHSBRange;

namespace RunescapeBot.BotPrograms
{
    /// <summary>
    /// A falconry kebbit.
    /// </summary>
    public class Kebbit
    {
        public static IColorFilter KebbitBlackSpot = RGBHSBRangeFactory.KebbitBlackSpot();
        public static IColorFilter KebbitWhiteSpot = RGBHSBRangeFactory.KebbitWhiteSpot();
        public static IColorFilter KebbitSpottedFur = RGBHSBRangeFactory.KebbitSpottedFur();
        public static IColorFilter KebbitDarkFur = RGBHSBRangeGroupFactory.KebbitDarkFur();
        public static IColorFilter KebbitDashingFur = RGBHSBRangeFactory.KebbitDashingFur();


        /// <summary>
        /// Creates a kebbit based on a single screenshot.
        /// </summary>
        /// <param name="gameScreen">The screenshot from which to create the kebbit.</param>
        /// <param name="playerLocation">The location of the plaer in the screenshot.</param>
        public Kebbit(Color[,] gameScreen, Cluster spots, Point playerLocation)
        {
            GameScreen = gameScreen;
            Location = spots;
            PlayerLocation = playerLocation;
        }


        /// <summary>
        /// The screenshot that is used to create this kebbit.
        /// </summary>
        protected Color[,] GameScreen { get; set; }

        /// <summary>
        /// The location of the player. Used to assess the value of a kebbit.
        /// </summary>
        protected Point PlayerLocation { get; set; }

        /// <summary>
        /// A cluster of blobs marking the location of the kebbit.
        /// </summary>
        public Cluster Location { get; set; }

        /// <summary>
        /// The type of kebbit (Spotted, Dark, Dashing, or Unknown).
        /// </summary>
        public KebbitType Type
        {
            get
            {
                if (_type == KebbitType.Unidentified)
                {
                    IdentifyKebbit();
                }
                return _type;
            }
        }
        protected KebbitType _type;

        public enum KebbitType : int
        {
            Unidentified,   //We have not tried to identify the kebbit yet.
            Unknown,        //We have tried and failed to identify the kebbit.
            Spotted,
            Dark,
            Dashing
        }

        /// <summary>
        /// The real types of kebbits that exist in the game (as opposed to failure state classifications).
        /// </summary>
        protected List<KebbitType> IdentifiableKebbits
        {
            get
            {
                if (_identifiableKebbits == null)
                {
                    _identifiableKebbits = new List<KebbitType>();
                    _identifiableKebbits.Add(KebbitType.Spotted);
                    _identifiableKebbits.Add(KebbitType.Dark);
                    _identifiableKebbits.Add(KebbitType.Dashing);
                }
                
                return _identifiableKebbits;
            }
        }
        protected static List<KebbitType> _identifiableKebbits;

        /// <summary>
        /// Gets the color filter for the fur of a type of kebbit.
        /// </summary>
        /// <param name="kebbitType">The type of kebbit.</param>
        /// <returns>A color filter for the given kebbit type.</returns>
        public static IColorFilter GetKebbitFilter(KebbitType kebbitType)
        {
            switch(kebbitType)
            {
                case KebbitType.Spotted:
                    return KebbitSpottedFur;
                case KebbitType.Dark:
                    return KebbitDarkFur;
                case KebbitType.Dashing:
                    return KebbitDashingFur;
                default:
                    return new RejectFilter();
            }
        }

        /// <summary>
        /// Attempts to identify the kebbit.
        /// </summary>
        /// <returns>True if the kebbit is identified as a known species of kebbit. False if the kebbit cannot be identified.</returns>
        public bool IdentifyKebbit()
        {
            int searchRadius = (int) (0.01 * GameScreen.GetLength(1));
            int left = Location.Center.X - searchRadius;
            int right = Location.Center.X + searchRadius;
            int top = Location.Center.Y - searchRadius;
            int bottom = Location.Center.Y + searchRadius;

            List<KebbitType> identifiableKebbits = IdentifiableKebbits;
            double furMatch;
            double bestFurMatch = 0.01;    //minimum threshold for a valid kebbit
            _type = KebbitType.Unknown;

            foreach (KebbitType kebbitType in IdentifiableKebbits)
            {
                furMatch = ImageProcessing.FractionalMatchPiece(GameScreen, GetKebbitFilter(kebbitType), left, right, top, bottom);
                if (furMatch > bestFurMatch)
                {
                    _type = kebbitType;
                    bestFurMatch = furMatch;
                }
            }
            
            return false;
        }

        /// <summary>
        /// The amount of experience awarded for killing this kebbit.
        /// </summary>
        protected double Experience
        {
            get
            {
                switch (Type)
                {
                    case KebbitType.Unknown:
                        return 0;
                    case KebbitType.Spotted:
                        return 104;
                    case KebbitType.Dark:
                        return 132;
                    case KebbitType.Dashing:
                        return 156;
                }
                return 0;
            }
        }

        /// <summary>
        /// The value of this kebbit. Used to determine the order to attempt to catch kebbits.
        /// </summary>
        public double Value
        {
            get
            {
                Point idealLocation = new Point((int)(PlayerLocation.X + 0.2 * GameScreen.GetLength(1)), (int)(PlayerLocation.Y + 0.1 * GameScreen.GetLength(1)));
                double distance = Geometry.DistanceBetweenPoints(Location.Center, idealLocation);
                double distanceCost = 0.2 * GameScreen.GetLength(1) + distance;
                double value = (1 / distanceCost) * Experience;
                return value;
            }
        }
    }

    /// <summary>
    /// Compares the value of two kebbits.
    /// </summary>
    public class KebbitComparer : IComparer<Kebbit>
    {
        /// <summary>
        /// Determines the sorting preference between two kebbits.
        /// </summary>
        /// <param name="a">A kebbit.</param>
        /// <param name="b">Another kebbit.</param>
        /// <returns>-1 if a sorts before b, 1 if b sorts before a, 0 otherwise.</returns>
        public int Compare(Kebbit a, Kebbit b)
        {
            if (a.Value > b.Value)
            {
                return -1;  //Sort a before b.
            }
            if (b.Value > a.Value)
            {
                return 1;   //Sort b before a.
            }
            else
            {
                return 0;   //No sorting preference.
            }
        }
    }
}
