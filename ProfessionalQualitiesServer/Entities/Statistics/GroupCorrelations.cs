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
                var personalData = pte.Tested.PersonalData;
                if (personalData == null)
                {
                    return false;
                }

                if (professionId > 0)
                {
                    return personalData.ProfessionId == professionId;
                }
                else if (professionId < 0)
                {
                    return personalData.ProfessionId != (-professionId);
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
            CorrelationValues = CorrelationValuesFactory.CountCorrelations(expertAssessments, points);                 
        }

        public GroupCorrelations(TestEntity testEntity, int scaleId)
            : this(testEntity, scaleId, 0) { }

        public GroupCorrelations(TestEntity testEntity, int scaleId, int professionId, string groupName)
            : this(testEntity, scaleId, professionId) => GroupName = groupName;

        public GroupCorrelations(TestEntity testEntity, int scaleId, string groupName)
            : this(testEntity, scaleId) => GroupName = groupName;

        public string GroupName { get; set; }
        public IEnumerable<CorrelationValue> CorrelationValues { get; set; }
        

        private bool WhereTestedEstimated(PassedTestEntity pte)
        {
            var personalData = pte.Tested.PersonalData;
            if (personalData != null) {
                return personalData.ExpertAssessment > -1;
            }
            return false;
        }
    }
}
