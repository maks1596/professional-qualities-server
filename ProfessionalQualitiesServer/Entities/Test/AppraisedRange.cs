namespace ProfessionalQualitiesServer
{
    public class AppraisedRange
    {
        public AppraisedRange()
        {
            LowerRangeValue = 0;
            UpperRangeValue = 0;
            Result = "";
        }

        public AppraisedRange(int lowerRangeValue, int upperRangeValue, string result)
        {
            LowerRangeValue = lowerRangeValue;
            UpperRangeValue = upperRangeValue;
            Result = result;
        }

        public AppraisedRange(EvaluationMapEntity evaluationMapEntity)
        {
            LowerRangeValue = evaluationMapEntity.LowerRangeValue;
            UpperRangeValue = evaluationMapEntity.UpperRangeValue;
            Result = evaluationMapEntity.Result;
        }

        public int LowerRangeValue { get; set; }
        public int UpperRangeValue { get; set; }
        public string Result { get; set; } 

        public EvaluationMapEntity ToEntity(ProfessionalQualitiesDbContext dbContext, 
                                            TestEntity testEntity, ScaleEntity scaleEntity)
        {
            var entity = new EvaluationMapEntity
            {
                Test = testEntity,
                Scale = scaleEntity,
                LowerRangeValue = LowerRangeValue,
                UpperRangeValue = UpperRangeValue,
                Result = Result
            };
            dbContext.EvaluationMaps.Add(entity);

            return entity;
        }
    }
}