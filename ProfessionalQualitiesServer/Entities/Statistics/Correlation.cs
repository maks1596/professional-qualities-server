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

            return sumXY / System.Math.Sqrt(sumX2 * sumY2);
        }
    }
}
