using System.Collections.Generic;
using System.Linq;

namespace ProfessionalQualitiesServer
{
    public class PartOfKey
    {
        public PartOfKey()
        {
            QuestionIndexes = new List<int>();
            AnswerOptionIndexes = new List<int>();
            Points = 0;
        }

        public PartOfKey(IEnumerable<int> questionIndexes, IEnumerable<int> answerOptionIndexes, int points)
        {
            QuestionIndexes = questionIndexes;
            AnswerOptionIndexes = answerOptionIndexes;
            Points = points;
        }

        public IEnumerable<int> QuestionIndexes { get; set; }
        public IEnumerable<int> AnswerOptionIndexes { get; set; }
        public int Points { get; set; }

        public IEnumerable<KeyEntity> ToEntities(ProfessionalQualitiesDbContext dbContext, 
                                                 TestEntity testEntity, ScaleEntity scaleEntity)
        {
            var questionEntities = testEntity.Questions;
            var answerOptionEntities = new List<AnswerOptionEntity>();
            if (!testEntity.AreAnswerOptionsUnique)
            {
                answerOptionEntities = testEntity.TestAnswerOptions.Select(tao => tao.AnswerOption).ToList();
            }

            foreach (var questionIndex in QuestionIndexes)
            {
                var questionEntity = questionEntities[questionIndex];
                if (testEntity.AreAnswerOptionsUnique)
                {
                    answerOptionEntities = questionEntity.QuestionAnswerOptions.Select(qao => qao.AnswerOption).ToList();
                }

                foreach (var answerOptionIndex in AnswerOptionIndexes)
                {
                    var keyEntity = new KeyEntity
                    {
                        Test = testEntity,
                        Scale = scaleEntity,
                        Question = questionEntity,
                        Answer = answerOptionEntities[answerOptionIndex],
                        Points = Points
                    };
                    dbContext.Keys.Add(keyEntity);

                    yield return keyEntity;
                }
            }
        }
    }
}