﻿using System.Collections.Generic;
using System.Linq;

namespace ProfessionalQualitiesServer.Entities.Statistics
{
    public class IndicatorGroup
    {
        public IndicatorGroup()
        {
            Name = "";
            Indicators = new List<Indicator>();
        }

        public string Name { get; set; }
        public List<Indicator> Indicators { get; set; }
    }

    public class DistributionIndicatorGroup : IndicatorGroup
    {
        public DistributionIndicatorGroup(IEnumerable<double> values) : base()
        {
            Name = Constants.DistributionIndicatorGroupName;

            Indicators.Add(new Indicator
            {
                Name = Constants.MinimumValueIndicatorName,
                Value = values.Min()
            });

            Indicators.Add(new Indicator
            {
                Name = Constants.MaximumValueIndicatorName,
                Value = values.Max()
            });
        }
    }

    public class DistributionCenterIndicatorGroup : IndicatorGroup
    {
        public DistributionCenterIndicatorGroup(IEnumerable<double> values) : base()
        {
            Name = Constants.DistributionCenterIndicatorGroupName;
            
            Indicators.Add(new Indicator
            {
                Name = Constants.ExpectedValueIndicatorName,
                Value = Math.M(values)
            });
            Indicators.Add(new Indicator
            {
                Name = Constants.MedianIndicatorName,
                Value = Math.Median(values)
            });
            Indicators.Add(new Indicator
            {
                Name = Constants.ModeIndicatorName,
                Value = Math.Mode(values)
            });
        }
    }

    public class VariationIndicatorGroup : IndicatorGroup
    {
        public VariationIndicatorGroup(IEnumerable<double> values)
        {
            Name = Constants.VariationIndicatorGroupName;

            Indicators.Add(new Indicator
            {
                Name = Constants.DataRangeIndicatorName,
                Value = Math.DataRange(values)
            });

            var d = Math.D(values);
            Indicators.Add(new Indicator
            {
                Name = Constants.VarianceIndicatorName,
                Value = d
            });

            Indicators.Add(new Indicator
            {
                Name = Constants.StandartDeviationIndicatorName,
                Value = Math.StandartDeviation(d)
            });

            Indicators.Add(new Indicator
            {
                Name = Constants.InterquartileRangeIndicatorName,
                Value = Math.InterquartileRange(values)
            });
        }
    }
}
