using System;
using System.Collections.Generic;
using System.Linq;

namespace ProfessionalQualitiesServer.Entities.Statistics
{
    public static class Correlation
    {
        public static double Covariance(IEnumerable<double> x, IEnumerable<double> y)
        {
            var xy = Math.Multiply(x, y);
            return Math.M(xy) - Math.M(x) * Math.M(y);
        }

        public static double PearsonCoefficient(IEnumerable<double> x, IEnumerable<double> y) => LinearCoefficient(x, y);
        public static double LinearCoefficient(IEnumerable<double> x, IEnumerable<double> y)
        {
            var count = x.Count();
            if (count != y.Count())
            {
                throw new ArgumentException();
            }

            var mX = Math.M(x);
            var mY = Math.M(y);
            double sumXY = .0, sumX2 = .0, sumY2 = .0;

            for (var i = 0; i < count; ++i)
            {
                var curX = x.ElementAt(i);
                var difX = curX - mX;

                var curY = y.ElementAt(i);
                var difY = curY - mY;

                sumXY += difX * difY;
                sumX2 += System.Math.Pow(difX, 2);
                sumY2 += System.Math.Pow(difY, 2);
            }

            if (sumXY == 0 || sumX2 == 0 || sumY2 == 0)
            {
                return 0;
            }
            else
            {
                return sumXY / System.Math.Sqrt(sumX2 * sumY2);
            }
        }

        public static double KendallCoefficient(IEnumerable<double> x, IEnumerable<double> y)
        {
            var count = x.Count();
            if (count != y.Count())
            {
                throw new ArgumentException();
            }

            var rankX = Rank(x);
            var rankY = Rank(y);

            var xy = rankX.Select((curX, index) => new { x = curX, y = rankY.ElementAt(index) });
            var sortedXY = xy.OrderBy(curXY => curXY.x);

            int p = 0, q = 0;

            for (int i = 0; i < count; ++i)
            {
                var curY = sortedXY.ElementAt(i).y;
                int coincidenceCount = 0, inversionCount = 0; 

                for (int j = i + 1; j < count; j++)
                {
                    if (y.ElementAt(j) > curY)
                    {
                        ++coincidenceCount;
                    }
                    else
                    {
                        ++inversionCount;
                    }
                }
                p += coincidenceCount;
                q += inversionCount;
            }

            return 2.0 * (p - q) / count / (count - 1);
        }

        private static IEnumerable<double> Rank(IEnumerable<double> values)
        {
            return values.Select(value => Rank(value, values)).ToList();
        }


        private static double Rank(double value, IEnumerable<double> values)
        {   // Знак сравнения роли не играет
            var greaterCount = values.Count(curValue => curValue < value);
            var equalCount = values.Count(curValue => curValue == value);
            var rank = Enumerable.Range(greaterCount, equalCount).Average();
            return rank;
        }
    }
}
