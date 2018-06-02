using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProfessionalQualitiesServer.Entities.Statistics
{
    public static class CorrelationValuesFactory
    {
        private static Dictionary<string, Func<IEnumerable<double>, IEnumerable<double>, double>> CorrelationFunctions = 
            new Dictionary<string, Func<IEnumerable<double>, IEnumerable<double>, double>>
        {
            { Constants.CovarianceName, Correlation.Covariance },
            { Constants.PearsonCoefficientName, Correlation.PearsonCoefficient }
        };

        public static IEnumerable<CorrelationValue> CountCorrelations(IEnumerable<double> x, IEnumerable<double> y)
        {
            foreach (var (name, func) in CorrelationFunctions)
            {
                var correlationValue = new CorrelationValue(name);
                try
                {
                    correlationValue.SetDoubleValue(func(x, y));
                }
                catch (Exception) { }
                yield return correlationValue;
            }
        }
    }
}
