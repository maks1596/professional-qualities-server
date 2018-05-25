using Newtonsoft.Json.Converters;

namespace ProfessionalQualitiesServer
{
    public class IsoDateConverter : IsoDateTimeConverter
    {
        public IsoDateConverter() => DateTimeFormat = "yyyy-MM-dd";
    }
}
