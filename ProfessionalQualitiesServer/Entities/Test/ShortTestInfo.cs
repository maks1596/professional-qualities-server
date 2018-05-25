namespace ProfessionalQualitiesServer
{
    public class ShortTestInfo
    {
        public ShortTestInfo() : this(0, "") { }

        public ShortTestInfo(int id, string name)
        {
            Id = id;
            Name = name;
        }

        public ShortTestInfo(TestEntity testEntity) : this(testEntity.Id, testEntity.Name) { }
        
        public int Id { get; set; }
        public string Name { get; set; }

        public virtual TestEntity ToEntity(ProfessionalQualitiesDbContext dbContext)
        {
            TestEntity entity = new TestEntity();

            entity.Id = Id;
            entity.Name = Name;

            return entity;
        }
    }
}