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

        public static double Median(IEnumerable<double> values)
        {
            if (values == null)
            {
                throw new ArgumentException(nameof(values));
            }

            var count = values.Count();
            if (count < 1)
            {
                throw new ArgumentOutOfRangeException(nameof(values));
            }

            var middlePoint = count / 2;
            var sortedValues = values.OrderBy(v => v);
            if (count % 2 == 0)
            {
                return (sortedValues.ElementAt(middlePoint - 1) + sortedValues.ElementAt(middlePoint)) / 2;
            }
            else
            {
                return sortedValues.ElementAt(middlePoint);
            }
        }

        public static double Mode(IEnumerable<double> values)
        {
            if (values == null)
            {
                throw new ArgumentException(nameof(values));
            }
            var valueCount = values.GroupBy(v => v)
                         .Select(gv => new { value = gv.Key, count = gv.Count() });
            var maxCount = valueCount.Max(vc => vc.count);
            return valueCount.Where(vc => vc.count == maxCount).First().value;
        }

        public static double Quantile(IEnumerable<double> values, double percent)
        {
            if (values == null)
            {
                throw new ArgumentException(nameof(values));
            }
            var count = values.Count();
            if (count < 1)
            {
                throw new ArgumentOutOfRangeException(nameof(values));
            }
            int index = (int)(percent * count);
            return values.OrderBy(v => v).ElementAt(index);
        }

        public static double DataRange(IEnumerable<double> values)
        {
            if (values == null)
            {
                throw new ArgumentException(nameof(values));
            }
            return values.Max() - values.Min();
        }

        public static double D(IEnumerable<double> values) => Variance(values);
        public static double Variance(IEnumerable<double> values) => Variance(values, M(values));

        public static double D(IEnumerable<double> values, double m) => Variance(values, m);
        public static double Variance(IEnumerable<double> values, double expectedValue)
        {
            return M(values.Select(x => System.Math.Pow(x - expectedValue, 2)));
        }

        public static double StandartDeviation(IEnumerable<double> values)
        {
            return StandartDeviation(D(values));
        }

        public static double StandartDeviation(double d)
        {
            return System.Math.Sqrt(d);
        }

        public static double InterquartileRange(IEnumerable<double> values)
        {
            return Quantile(values, 0.75) - Quantile(values, 0.25);
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
