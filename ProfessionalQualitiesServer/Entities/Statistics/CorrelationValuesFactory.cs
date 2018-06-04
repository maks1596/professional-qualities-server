using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProfessionalQualitiesServer.Entities.Statistics
{
    public static class CorrelationValuesFactory
    {
        private static readonly Dictionary<string, Func<IEnumerable<double>, IEnumerable<double>, double>> CorrelationFunctions = 
            new Dictionary<string, Func<IEnumerable<double>, IEnumerable<double>, double>>
        {
            { Constants.CovarianceName, Correlation.Covariance },
            { Constants.PearsonCoefficientName, Correlation.PearsonCoefficient }
        };

        private static readonly Dictionary<string, string> CorrelationFullNames =
            new Dictionary<string, string>
        {
            { Constants.CovarianceName, Constants.CovarianceName },
            { Constants.PearsonCoefficientName, Constants.PearsonCoefficientFullName }
        };

        public static IEnumerable<CorrelationValue> CountCorrelations(IEnumerable<double> x, IEnumerable<double> y)
        {
            foreach (var (name, func) in CorrelationFunctions)
            {
                var correlationValue = new CorrelationValue(name, CorrelationFullNames[name]);
                try
                {
                    var result = func(x, y);
                    correlationValue.SetDoubleValue(result);
                }
                catch (Exception) { }
                yield return correlationValue;
            }
        }
    }
}
