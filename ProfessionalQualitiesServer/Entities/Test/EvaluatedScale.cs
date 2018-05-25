using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProfessionalQualitiesServer.Entities
{
    public class EvaluatedScale
    {
        public EvaluatedScale()
        {
            ScaleId = 0;
            Points = 0;
        }

        public int ScaleId { get; set; }
        public int Points { get; set; }
    }
}
