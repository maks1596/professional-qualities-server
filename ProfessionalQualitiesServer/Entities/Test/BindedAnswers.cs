using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProfessionalQualitiesServer.Entities
{
    public class BindedAnswers
    {
        public BindedAnswers()
        {
            UserId = 0;
            TestId = 0;
            Answers = new List<Answer>();
        }

        public int UserId { get; set; }
        public int TestId { get; set; }
        public IEnumerable<Answer> Answers { get; set; }
    }
}
