using System;
using System.Collections.Generic;
using System.Linq;
using System.Drawing;

namespace RunescapeBot.UITools
{
    /// <summary>
    /// Spline interpolation class.
    /// </summary>
    public class SplineInterpolator
    {
        private readonly double[] _keys;

        private readonly double[] _values;

        private readonly double[] _h;

        private readonly double[] _a;

        /// <summary>
        /// Simple class constructor
        /// </summary>
        /// <param name="start">Start point for the spline</param>
        /// <param name="finish">End point for the spline</param>
        public SplineInterpolator(Point start, Point end)
        {
            Random rng = new Random();
            List<double> nodesX = new List<double>();
            List<double> nodesY = new List<double>();
            double rise, run, distance, x, y;
            int midpoints, variationX, variationY;

            rise = end.Y - start.Y;
            run = end.X - start.X;
            variationX = (int) Math.Abs(rise / 30.0);
            variationY = (int) Math.Abs(run / 30.0);
            distance = Math.Sqrt(Math.Pow(run, 2.0) + Math.Pow(rise, 2.0));
            if (distance < 20.0)
            {
                midpoints = 0;
            }
            else
            {
                midpoints = (int)Math.Max(1, distance / 500.0);
            }

            nodesX.Add(start.X);
            nodesY.Add(start.Y);
            for (int i = 1; i <= midpoints; i++)
            {
                x = start.X + ((i / (double)(midpoints+1)) * run) + rng.Next(-variationX, variationX + 1);
                y = start.Y + ((i / (double)(midpoints+1)) * rise) + rng.Next(-variationY, variationY + 1);
                nodesX.Add(x);
                nodesY.Add(y);
            }
            nodesX.Add(end.X);
            nodesY.Add(end.Y);

            var n = nodesX.Count;

            if (n < 2)
            {
                throw new ArgumentException("At least two point required for interpolation.");
            }

            nodesX.Sort();
            _keys = nodesX.ToArray();
            nodesY.Sort();
            _values = nodesY.ToArray();
            _a = new double[n];
            _h = new double[n];

            for (int i = 1; i < n; i++)
            {
                _h[i] = _keys[i] - _keys[i - 1];
            }

            if (n > 2)
            {
                var sub = new double[n - 1];
                var diag = new double[n - 1];
                var sup = new double[n - 1];

                for (int i = 1; i <= n - 2; i++)
                {
                    diag[i] = (_h[i] + _h[i + 1]) / 3;
                    sup[i] = _h[i + 1] / 6;
                    sub[i] = _h[i] / 6;
                    _a[i] = (_values[i + 1] - _values[i]) / _h[i + 1] - (_values[i] - _values[i - 1]) / _h[i];
                }

                SolveTridiag(sub, diag, sup, ref _a, n - 2);
            }
        }

        /// <summary>
        /// Gets interpolated value for specified argument.
        /// </summary>
        /// <param name="key">Argument value for interpolation. Must be within 
        /// the interval bounded by lowest ang highest <see cref="_keys"/> values.</param>
        public double GetValue(double key)
        {
            int gap = 0;
            var previous = double.MinValue;

            // At the end of this iteration, "gap" will contain the index of the interval
            // between two known values, which contains the unknown z, and "previous" will
            // contain the biggest z value among the known samples, left of the unknown z
            if (key <= _keys[0])
            {
                gap = 1;
                previous = _keys[0];
            }

            for (int i = 0; i < (_keys.Length - 1); i++)
            {
                if ((_keys[i] < key) && (_keys[i] > previous))
                {
                    previous = _keys[i];
                    gap = i + 1;
                }
            }

            var x1 = key - previous;
            var x2 = _h[gap] - x1;

            return ((-_a[gap - 1] / 6 * (x2 + _h[gap]) * x1 + _values[gap - 1]) * x2 +
                (-_a[gap] / 6 * (x1 + _h[gap]) * x2 + _values[gap]) * x1) / _h[gap];
        }


        /// <summary>
        /// Solve linear system with tridiagonal n*n matrix "a"
        /// using Gaussian elimination without pivoting.
        /// </summary>
        private static void SolveTridiag(double[] sub, double[] diag, double[] sup, ref double[] b, int n)
        {
            int i;

            for (i = 2; i <= n; i++)
            {
                sub[i] = sub[i] / diag[i - 1];
                diag[i] = diag[i] - sub[i] * sup[i - 1];
                b[i] = b[i] - sub[i] * b[i - 1];
            }

            b[n] = b[n] / diag[n];

            for (i = n - 1; i >= 1; i--)
            {
                b[i] = (b[i] - sup[i] * b[i + 1]) / diag[i];
            }
        }
    }
}