using System;

namespace ProfessionalQualitiesServer.Entities.Statistics
{
    public class CorrelationValue
    {
        public CorrelationValue() : this("") { }
        public CorrelationValue(string name) : this(name, "") { }
        public CorrelationValue(string name, string value)
        {
            Name = name;
            Value = value;
        }

        public CorrelationValue(string name, double value) 
            : this(name, value.ToString()) { }

        public string Name { get; set; }
        public string Value { get; set; }

        public double GetDoubleValue() => Convert.ToDouble(Value);
        public void SetDoubleValue(double value) => Value = value.ToString("0.####");
    }
}