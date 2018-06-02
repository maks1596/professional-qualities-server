using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProfessionalQualitiesServer.Entities.Statistics
{
    public class GroupCorrelations
    {
        public GroupCorrelations(TestEntity testEntity, int scaleId, int professionId = 0)
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

        }

        public GroupCorrelations(TestEntity testEntity, int scaleId, int professionId, string groupName)
            : this(testEntity, scaleId, professionId) => GroupName = groupName;

        public string GroupName { get; set; }
        public List<CorrelationValue> CorrelationValues { get; set; }

        private bool WhereTestedEstimated(PassedTestEntity pte)
        {
            return pte.Tested.PersonalData.ExpertAssessment > -1;
        }
    }
}
