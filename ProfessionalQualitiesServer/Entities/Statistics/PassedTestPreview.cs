using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProfessionalQualitiesServer.Entities.Statistics
{
    public class PassedTestPreview
    {
        public PassedTestPreview(TestEntity testEntity)
        {
            Id = testEntity.Id;
            Name = testEntity.Name;

            if (testEntity.PassedTests != null)
            {
                NumberOfPasses = testEntity.PassedTests.Count;
            }
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public int NumberOfPasses { get; set; }

    }
}
