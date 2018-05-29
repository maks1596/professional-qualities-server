﻿using System.Collections.Generic;

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
        }
    }

    public class VariationIndicatorGroup : IndicatorGroup
    {
        public VariationIndicatorGroup(IEnumerable<double> values)
        {
            Name = Constants.VariationIndicatorGroupName;

            Indicators.Add(new Indicator
            {
                Name = Constants.VarianceIndicatorName,
                Value = Math.D(values)
            });
        }
    }
}