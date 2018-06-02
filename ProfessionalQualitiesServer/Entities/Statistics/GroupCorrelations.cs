using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProfessionalQualitiesServer.Entities.Statistics
{
    public class GroupCorrelations
    {
        public GroupCorrelations(TestEntity testEntity, int scaleId, int professionId)
        {
            bool WhereProfessionId(PassedTestEntity pte)
            {
                if (professionId > 0)
                {
                    return pte.Tested.PersonalData.Profession.Id == professionId;
                }
                else if (professionId < 0)
                {
                    return pte.Tested.PersonalData.Profession.Id != (-professionId);
                }
                return true;
            }

            var resultEntities = testEntity.PassedTests
                                           .Where(WhereProfessionId)
                                           .Where(WhereTestedEstimated)
                                           .SelectMany(pte => pte.Results)
                                           .Where(re => re.ScaleId == scaleId);

            var expertAssessments = resultEntities.Select(re => Convert.ToDouble(re.PassedTest.Tested.PersonalData.ExpertAssessment));
            var points = resultEntities.Select(re => Convert.ToDouble(re.Points));

            CorrelationValues = new List<CorrelationValue>();
            CorrelationValues.Add(new CorrelationValue
            {
                Name = Constants.CovarianceName,
                Value = Correlation.Covariance(expertAssessments, points)
            });
            CorrelationValues.Add(new CorrelationValue
            {
                Name = Constants.PearsonCoefficientName,
                Value = Correlation.PearsonCoefficient(expertAssessments, points)
            });
        }

        public GroupCorrelations(TestEntity testEntity, int scaleId)
            : this(testEntity, scaleId, 0) { }

        public GroupCorrelations(TestEntity testEntity, int scaleId, int professionId, string groupName)
            : this(testEntity, scaleId, professionId) => GroupName = groupName;

        public GroupCorrelations(TestEntity testEntity, int scaleId, string groupName)
            : this(testEntity, scaleId) => GroupName = groupName;

        public string GroupName { get; set; }
        public List<CorrelationValue> CorrelationValues { get; set; }

        private bool WhereTestedEstimated(PassedTestEntity pte)
        {
            return pte.Tested.PersonalData.ExpertAssessment > -1;
        }
    }
}
