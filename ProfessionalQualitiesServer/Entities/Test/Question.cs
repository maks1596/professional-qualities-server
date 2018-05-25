using System;
using System.Collections.Generic;
using System.Linq;

namespace ProfessionalQualitiesServer
{
    public class Question
    {
        public Question()
        {
            AnswerOptions = new List<AnswerOption>();
        }

        public Question(int id, string formulation, IEnumerable<AnswerOption> answerOptions)
        {
            Id = id;
            Formulation = formulation;
            AnswerOptions = answerOptions;
        }

        public Question(QuestionEntity questionEntity)
            : this(questionEntity.Id, questionEntity.Formulation, 
                   questionEntity.QuestionAnswerOptions
                  .OrderBy(qao => qao.Order)
                  .Select(qao => qao.AnswerOption)
                  .Select(aoe => new AnswerOption(aoe)))
        { }
        
        public int Id { get; set; }
        public string Formulation { get; set; }
        public IEnumerable<AnswerOption> AnswerOptions { get; set; }

        public QuestionEntity ToEntity(ProfessionalQualitiesDbContext dbContext, int order, bool areAnswerOptionsUnique = true)
        {
            QuestionEntity entity = new QuestionEntity
            {
                Order = order,
                Formulation = Formulation
            };

            if (areAnswerOptionsUnique)
            {
                var answerOptionEntities = AnswerOptions.Select(ao => ao.ToEntity(dbContext)).ToList();
                for (int i = 0; i < answerOptionEntities.Count; ++i)
                {
                    dbContext.QuestionsAnswerOptions.Add(new QuestionAnswerOption
                    {
                        Question = entity,
                        AnswerOption = answerOptionEntities[i],
                        Order = i
                    });
                }
            }
            dbContext.Questions.Add(entity);

            return entity;
        }
    }
}