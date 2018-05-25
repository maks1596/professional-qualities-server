using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProfessionalQualitiesServer.Entities
{
    public class ScaleResult
    {
        public ScaleResult(EvaluationMapEntity evaluationMapEntity, int points)
        {
            ScaleId = evaluationMapEntity.ScaleId;
            ScaleName = evaluationMapEntity.Scale.Name;
            Points = points;
            Result = evaluationMapEntity.Result;
        }

        public int ScaleId { get; set; }
        public string ScaleName { get; set; }
        public int Points { get; set; }
        public string Result { get; set; }
    }
}
