using System;
using System.Linq;

namespace ProfessionalQualitiesServer
{
    public class AnswerOption
    {
        public AnswerOption()
        {
            Id = 0;
            Formulation = "";
        }

        public AnswerOption(int id, string formulation)
        {
            Id = id;
            Formulation = formulation;
        }

        public AnswerOption(AnswerOptionEntity answerOptionEntity)
            : this(answerOptionEntity.Id, answerOptionEntity.Formulation) { }
        
        public int Id { get; set; }
        public string Formulation { get; set; }

        public AnswerOptionEntity ToEntity(ProfessionalQualitiesDbContext dbContext)
        {
            var entity = dbContext.AnswerOptions
                        .SingleOrDefault(aoe => aoe.Formulation == Formulation);
            if (entity == null)
            {
                entity = new AnswerOptionEntity
                {
                    Formulation = Formulation
                };
                dbContext.AnswerOptions.Add(entity);
                dbContext.SaveChanges();
            }

            return entity;
        }
    }
}