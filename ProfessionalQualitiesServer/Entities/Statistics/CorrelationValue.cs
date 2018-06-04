using System;

namespace ProfessionalQualitiesServer.Entities.Statistics
{
    public class CorrelationValue
    {
        public CorrelationValue() : this("") { }
        public CorrelationValue(string name) : this(name, name) { }
        public CorrelationValue(string name, string fullName) : this(name, fullName, "") { }
        public CorrelationValue(string name, string fullName, string value)
        {
            Name = name;
            FullName = fullName;
            Value = value;
        }

        public CorrelationValue(string name, double value) 
            : this(name, value.ToString()) { }

        public string Name { get; set; }
        public string FullName { get; set; }
        public string Value { get; set; }

        public double GetDoubleValue() => Convert.ToDouble(Value);
        public void SetDoubleValue(double value) => Value = value.ToString("0.####");
    }
}