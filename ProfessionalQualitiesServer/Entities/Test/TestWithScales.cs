using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProfessionalQualitiesServer
{
    public class TestWithScales : Test
    {
        public TestWithScales() : base() { }

        public TestWithScales(int id, string name) : base(id, name) { }

        public TestWithScales(TestEntity testEntity) : base(testEntity)
        {
            Scales = testEntity.GetScales()
                               .Select(se => new Scale(se, testEntity))
                               .OrderBy(scale => scale.Name);
        }

        public IEnumerable<Scale> Scales { get; set; }

        public override TestEntity ToEntity(ProfessionalQualitiesDbContext dbContext)
        {
            var entity = base.ToEntity(dbContext);
            foreach (var scale in Scales)
            {
                scale.ToEntity(dbContext, entity);
            }
            return entity;
        }
    }
}
