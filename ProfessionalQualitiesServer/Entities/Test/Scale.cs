using System;
using System.Collections.Generic;
using System.Linq;

namespace ProfessionalQualitiesServer
{
    public class Scale
    {
        public Scale()
        {
            Id = 0;
            Name = "";
            Key = new List<PartOfKey>();
            EvaluationMap = new List<AppraisedRange>();
        }

        public Scale(int id, string name, IEnumerable<PartOfKey> key, IEnumerable<AppraisedRange> evaluationMap)
        {
            Id = id;
            Name = name;
            Key = key;
            EvaluationMap = evaluationMap;
        }

        public Scale(ScaleEntity scaleEntity, TestEntity testEntity)
        {
            Id = scaleEntity.Id;
            Name = scaleEntity.Name;

            Key = MakeKey(scaleEntity, testEntity);
            if (testEntity.AreAnswerOptionsUnique)
            {
                Key = Key.OrderBy(pok => pok.QuestionIndexes.First());
            }
            else
            {
                Key = Key.OrderBy(pok => pok.AnswerOptionIndexes.First());
            }

            EvaluationMap = testEntity.EvaluationMap
                            .Where(eme => eme.ScaleId == Id)
                            .Select(eme => new AppraisedRange(eme))
                            .OrderBy(ar => ar.LowerRangeValue)
                            .OrderBy(ar => ar.UpperRangeValue);
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public IEnumerable<PartOfKey> Key { get; set; }
        public IEnumerable<AppraisedRange> EvaluationMap { get; set; }

        public ScaleEntity ToEntity(ProfessionalQualitiesDbContext dbContext, TestEntity testEntity)
        {
            ScaleEntity scaleEntity = new ScaleEntity();

            if (dbContext.Scales.Any(se => se.Name == Name))
            {
                scaleEntity = dbContext.Scales
                            .Single(se => se.Name == Name);
            }
            else
            {
                scaleEntity.Name = Name;
                dbContext.Scales.Add(scaleEntity);
            }

            GetKeyEntity(dbContext, testEntity, scaleEntity).ToList();
            GetEvaluationMapEntity(dbContext, testEntity, scaleEntity).ToList();

            return scaleEntity;
        }

        private IEnumerable<KeyEntity> GetKeyEntity(ProfessionalQualitiesDbContext dbContext, 
                                                    TestEntity testEntity, ScaleEntity scaleEntity)
        {
            foreach (var partOfKey in Key)
            {
                var keyEntities = partOfKey.ToEntities(dbContext, testEntity, scaleEntity);
                foreach (var keyEntity in keyEntities)
                {
                    yield return keyEntity;
                }
            }
        }

        private IEnumerable<EvaluationMapEntity> GetEvaluationMapEntity(ProfessionalQualitiesDbContext dbContext,
                                                                        TestEntity testEntity, ScaleEntity scaleEntity)
        {
            foreach (var appraisedRange in EvaluationMap)
            {
                yield return appraisedRange.ToEntity(dbContext, testEntity, scaleEntity);
            }
        }

        private IEnumerable<PartOfKey> MakeKey(ScaleEntity scaleEntity, TestEntity testEntity)
        {
            var keyEntites = testEntity.Key
                .Where(ke => ke.ScaleId == scaleEntity.Id);
            var evaluatedAnswers = KeyToEvaluatedAnswers(keyEntites, testEntity);
            
            if (testEntity.AreAnswerOptionsUnique)
            {
                var groupedEvaluatedAnswers = evaluatedAnswers
                    .GroupBy(ea => new { ea.QuestionIndex, ea.Points }, ea => ea.AnswerIndex);

                foreach (var group in groupedEvaluatedAnswers)
                {
                    var answerOptionIndexes = new List<int>();

                    foreach (var answerOptionIndex in group)
                    {
                        answerOptionIndexes.Add(answerOptionIndex);
                    }

                    yield return new PartOfKey
                    {
                        QuestionIndexes = new List<int> { group.Key.QuestionIndex },
                        AnswerOptionIndexes = answerOptionIndexes.OrderBy(idx => idx),
                        Points = group.Key.Points
                    };
                }
            }
            else
            {
                var groupedEvaluatedAnswers = evaluatedAnswers
                   .GroupBy(ea => new { ea.AnswerIndex, ea.Points }, ea => ea.QuestionIndex);

                foreach (var group in groupedEvaluatedAnswers)
                {
                    var questionIndexes = new List<int>();

                    foreach (var questionIndex in group)
                    {
                        questionIndexes.Add(questionIndex);
                    }

                    yield return new PartOfKey
                    {
                        AnswerOptionIndexes = new List<int> { group.Key.AnswerIndex },
                        QuestionIndexes = questionIndexes.OrderBy(idx => idx),
                        Points = group.Key.Points
                    };
                }           
            }
        }

        private IEnumerable<EvaluatedAnswer> KeyToEvaluatedAnswers(IEnumerable<KeyEntity> keyEntities, TestEntity testEntity)
        {
            var orderedAnswerOptions = testEntity.TestAnswerOptions
                .Select(tao => new { tao.Order, tao.AnswerOptionId });

            foreach (var keyEntity in keyEntities)
            {
                var currentQuestion = testEntity.Questions
                    .Single(qe => qe.Id == keyEntity.QuestionId);

                if (testEntity.AreAnswerOptionsUnique)
                {
                    orderedAnswerOptions = currentQuestion
                                                  .QuestionAnswerOptions
                                                  .Select(qao => new { qao.Order, qao.AnswerOptionId });
                }
                int answerOptionIndex = orderedAnswerOptions
                    .Single(oao => oao.AnswerOptionId == keyEntity.AnswerId)
                    .Order;

                yield return new EvaluatedAnswer
                {
                    AnswerIndex = answerOptionIndex,
                    QuestionIndex = currentQuestion.Order,
                    Points = keyEntity.Points
                };
            }
        }
    }
}