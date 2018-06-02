using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProfessionalQualitiesServer.Entities
{
    public class TestWithStatus : ShortTestInfo
    {
        public TestWithStatus() : base()
        {
            Status = Constants.NotPassedTestStatusString;
        }

        public TestWithStatus(TestEntity testEntity, int userId) : base(testEntity)
        {
            if (testEntity.PassedTests != null &&
                testEntity.PassedTests.Any(pte => pte.TestedId == userId))
            {
                Status = Constants.PassedTestStatusString;
            }
            else
            {
                Status = Constants.NotPassedTestStatusString;
            }
        }

        public string Status { get; set; }
    }
}
