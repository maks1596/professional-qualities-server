using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProfessionalQualitiesServer.Entities
{
    public class Answer
    {
        public Answer()
        {
            QuestionId = 0;
            AnswerId = 0;
        }

        public int QuestionId { get; set; }
        public int AnswerId { get; set; }
    }
}
