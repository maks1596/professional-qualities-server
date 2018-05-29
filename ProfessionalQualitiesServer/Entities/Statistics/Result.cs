using System;
using System.Collections.Generic;
using System.Linq;

namespace ProfessionalQualitiesServer.Entities.Statistics
{
    public class Result
    {
        public Result(string formulation, IEnumerable<ResultEntity> resultEntities, int numberOfResults)
        {
            Formulation = formulation;
            Times = resultEntities.Count();
            Frequency = Convert.ToDouble(Times) / numberOfResults;

            IndicatorGroups = new List<IndicatorGroup>();
            var points = resultEntities.Select(re => Convert.ToDouble(re.Points));
            IndicatorGroups.Add(new DistributionCenterIndicatorGroup(points));
            IndicatorGroups.Add(new VariationIndicatorGroup(points));
        }

        public string Formulation { get; set; }
        public int Times { get; set; }
        public double Frequency { get; set; }

        public List<IndicatorGroup> IndicatorGroups { get; set; }
    }
}
