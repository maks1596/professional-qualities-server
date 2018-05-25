using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProfessionalQualitiesServer.Entities
{
    public class TestWithStatus : ShortTestInfo
    {
        const string kPassedTestStatusString = "Passed";
        const string kNotPassedTestStatusString = "NotPassed";

        public TestWithStatus() : base()
        {
            Status = kNotPassedTestStatusString;
        }

        public TestWithStatus(TestEntity testEntity) : base(testEntity)
        {
            if (testEntity.PassedTests != null && testEntity.PassedTests.Count > 0)
            {
                Status = kPassedTestStatusString;
            }
            else
            {
                Status = kNotPassedTestStatusString;
            }
        }

        public string Status { get; set; }
    }
}
