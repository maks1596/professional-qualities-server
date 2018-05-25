using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProfessionalQualitiesServer.Entities.Statistics
{
    public class Test : PassedTestPreview
    {
        public Test(TestEntity testEntity) : base(testEntity)
        {
            Scales = testEntity.GetScales().Select(se => new Scale(testEntity, se));
        }

        public IEnumerable<Scale> Scales { get; set; }
    }
}
