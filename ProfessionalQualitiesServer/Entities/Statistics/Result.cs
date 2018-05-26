namespace ProfessionalQualitiesServer.Entities.Statistics
{
    public class Result
    {
        public string Formulation { get; set; }
        public int Times { get; set; }
        public double Frequency { get; set; }

        public double ExpectedPoints { get; set; }
        public double Variance { get; set; }
    }
}
