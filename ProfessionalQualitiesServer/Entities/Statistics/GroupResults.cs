using System;
using System.Collections.Generic;
using System.Linq;

namespace ProfessionalQualitiesServer.Entities.Statistics
{
    public class GroupResults
    {
        public GroupResults(string groupName, IEnumerable<ResultEntity> resultEntities)
        {
            GroupName = groupName;
            Results = MakeResults(resultEntities);            
        }

        public string GroupName { get; set; }
        public IEnumerable<Result> Results { get; set; }

        public static IEnumerable<Result> MakeResults(IEnumerable<ResultEntity> resultEntities)
        {
            var groupedResultEntities = resultEntities.GroupBy(re => re.Result);
            int numberOfResults = resultEntities.Count();

            foreach (var group in groupedResultEntities)
            {
                int times = group.Count();
                var points = group.Select(re => Convert.ToDouble(re.Points));
                var m = Math.M(points);

                yield return new Result
                {
                    Formulation = group.Key,
                    Times = times,
                    ExpectedPoints = m,
                    Variance = Math.D(points, m),
                    Frequency = Convert.ToDouble(times) / numberOfResults
                };
            }
        }
    }
}
