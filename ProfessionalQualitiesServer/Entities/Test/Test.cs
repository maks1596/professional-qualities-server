using System;
using System.Collections.Generic;
using System.Linq;

namespace ProfessionalQualitiesServer
{
    public class Test : ShortTestInfo
    {
        public Test() : base()
        {
            Instruction = "";
            Questions = new List<Question>();
            GeneralAnswerOptions = new List<AnswerOption>();
        }

        public Test(int id, string name) : base(id, name) {}

        public Test(TestEntity testEntity) : base(testEntity)
        {
            Instruction = testEntity.Instruction;
            Questions = testEntity.Questions
                .OrderBy(qe => qe.Order)
                .Select(qe => new Question(qe));
            GeneralAnswerOptions = testEntity.TestAnswerOptions
                .OrderBy(tao => tao.Order)
                .Select(tao => tao.AnswerOption)
                .Select(aoe => new AnswerOption(aoe));
        }

        public string Instruction { get; set; }
        public IEnumerable<Question> Questions { get; set; }
        public IEnumerable<AnswerOption> GeneralAnswerOptions { get; set; }

        public override TestEntity ToEntity(ProfessionalQualitiesDbContext dbContext)
        {
            var entity = base.ToEntity(dbContext);

            entity.Instruction = Instruction;
            entity.AreAnswerOptionsUnique = GeneralAnswerOptions.Count() == 0;

            if (entity.AreAnswerOptionsUnique)
            {
                entity.Questions = Questions.Select((question, index) => question.ToEntity(dbContext, index)).ToList();
            }
            else
            {
                entity.Questions = Questions.Select((question, index) => question.ToEntity(dbContext, index, false)).ToList();
                var generalAnswerOptionEntities = GeneralAnswerOptions.Select(ao => ao.ToEntity(dbContext)).ToList();

                for (int i = 0; i < generalAnswerOptionEntities.Count; ++i)
                {
                    dbContext.TestsAnswerOptions.Add(new TestAnswerOption
                    {
                        Test = entity,
                        Order = i,
                        AnswerOption = generalAnswerOptionEntities[i]
                    });
                }
            }

            return entity;
        }
    }
}