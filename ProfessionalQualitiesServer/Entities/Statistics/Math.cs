﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProfessionalQualitiesServer.Entities.Statistics
{
    internal static class Math
    {
        public static double M(IEnumerable<int> values) => ExpectedValue(values);
        public static double ExpectedValue(IEnumerable<int> values)
        {
            if (values == null)
            {
                throw new ArgumentNullException(nameof(values));
            }

            return values.Average();
        }

        public static double M(IEnumerable<double> values) => ExpectedValue(values);
        public static double ExpectedValue(IEnumerable<double> values)
        {
            if (values == null)
            {
                throw new ArgumentNullException(nameof(values));
            }

            return values.Average();
        }


        public static double D(IEnumerable<double> values) => Variance(values);
        public static double Variance(IEnumerable<double> values) => Variance(values, M(values));

        public static double D(IEnumerable<double> values, double m) => Variance(values, m);
        public static double Variance(IEnumerable<double> values, double expectedValue)
        {
            return M(values.Select(x => System.Math.Pow(x - expectedValue, 2)));
        }

        public static IEnumerable<double> Multiply(IEnumerable<double> x, IEnumerable<double> y)
        {
            var count = x.Count();
            if (count != y.Count())
            {
                throw new ArgumentException();
            }

            for (var i = 0; i < count; ++i)
            {
                yield return x.ElementAt(i) * y.ElementAt(i);
            }
        }
    }
}
