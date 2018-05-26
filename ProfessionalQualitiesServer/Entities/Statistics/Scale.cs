﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace ProfessionalQualitiesServer.Entities.Statistics
{
    public class Scale
    {
        public Scale(TestEntity testEntity, ScaleEntity scaleEntity)
        {
            Id = scaleEntity.Id;
            Name = scaleEntity.Name;

            var groupedResultEntities = testEntity.GetResults()
                                                  .Where(re => re.ScaleId == scaleEntity.Id)
                                                  .GroupBy(re => re.PassedTest
                                                                   .Tested
                                                                   .PersonalData
                                                                   .Profession
                                                                   .Name == Constants.ProgrammerProfessionString
                                                           );
            foreach (var group in groupedResultEntities)
            {
                if (group.Key)  // Программист
                {
                    ProgrammersResults = MakeResults(group);
                }
                else            // Не программист
                {
                    NonProgrammersResults = MakeResults(group);
                }
            }
        }


        public int Id { get; set; }
        public string Name { get; set; }
        public IEnumerable<Result> ProgrammersResults { get; set; }
        public IEnumerable<Result> NonProgrammersResults { get; set; }


        private static IEnumerable<Result> MakeResults(IEnumerable<ResultEntity> resultEntities)
        {
            var groupedResultEntities = resultEntities.GroupBy(re => re.Result);
            int numberOfResults = resultEntities.Count();

            foreach (var group in groupedResultEntities)
            {
                int times = group.Count();
                var points = group.Select(re => Convert.ToDouble(re.Points));
                var m = Math.M(points);

                yield return new Result
                {
                    Formulation = group.Key,
                    Times = times,
                    ExpectedPoints = m,
                    Variance = Math.D(points, m),
                    Frequency = Convert.ToDouble(times) / numberOfResults
                };
            }
        }

        private static IEnumerable<Result> MakeResultsForScale(IEnumerable<ResultEntity> resultEntities, int scaleId)
        {
            return MakeResults(resultEntities.Where(re => re.ScaleId == scaleId));
        }


        //  :: Расчёт коеффицентов корреляции ::
        //  :: Ковариация ::
        public static double CountCovariance(TestEntity testEntity, int firstScaleId, int secondScaleId)
        {
            var testResults = testEntity.GetResults();
            return CountCovariance(testResults, firstScaleId, secondScaleId);
        }

        public static double CountCovariance(TestEntity testEntity, int firstScaleId, int secondScaleId, string professionName)
        {
            var testResultsForProfession = testEntity.GetResults()
                                                     .Where(re => re.PassedTest
                                                                    .Tested
                                                                    .PersonalData
                                                                    .Profession
                                                                    .Name == professionName);
            return CountCovariance(testResultsForProfession, firstScaleId, secondScaleId);
        }

        private static double CountCovariance(IEnumerable<ResultEntity> testResults, int firstScaleId, int secondScaleId)
        {
            var firstScaleResults = MakeResultsForScale(testResults, firstScaleId);
            var secondScaleResults = MakeResultsForScale(testResults, secondScaleId);
            return CountCorrelationCoefficient(firstScaleResults, firstScaleResults, Correlation.Covariance);
        }


        //  :: Коеффициент Пирсона ::
        public static double CountPearsonCorrelationCoefficient(TestEntity testEntity, int firstScaleId, int secondScaleId)
        {
            var testResults = testEntity.GetResults();
            return CountPearsonCorrelationCoefficient(testResults, firstScaleId, secondScaleId);
        }

        public static double CountPearsonCorrelationCoefficient(TestEntity testEntity, int firstScaleId, int secondScaleId, string professionName)
        {
            var testResultsForProfession = testEntity.GetResults()
                                                     .Where(re => re.PassedTest
                                                                    .Tested
                                                                    .PersonalData
                                                                    .Profession
                                                                    .Name == professionName);
            return CountPearsonCorrelationCoefficient(testResultsForProfession, firstScaleId, secondScaleId);
        }

        private static double CountPearsonCorrelationCoefficient(IEnumerable<ResultEntity> testResults, int firstScaleId, int secondScaleId)
        {
            var firstScaleResults = MakeResultsForScale(testResults, firstScaleId);
            var secondScaleResults = MakeResultsForScale(testResults, secondScaleId);
            return CountCorrelationCoefficient(firstScaleResults, firstScaleResults, Correlation.PearsonCoefficient);
        }

        // :: Расчёт функции корреляции ::

        private static double CountCorrelationCoefficient(IEnumerable<ResultEntity> results,
                                                          int firstScaleId, 
                                                          int secondScaleId,
                                                          Func<IEnumerable<double>, IEnumerable<double>, double> correlationFunction)
        {
            var firstScaleResultPoints = results.Where(re => re.ScaleId == firstScaleId)
                                                .Select(re => Convert.ToDouble(re.Points));
            var secondScaleResultPoints = results.Where(re => re.ScaleId == secondScaleId)
                                                 .Select(re => Convert.ToDouble(re.Points));

            return correlationFunction(firstScaleResultPoints, secondScaleResultPoints);
        }

        private static double CountCorrelationCoefficient(IEnumerable<Result> firstResults,
                                                          IEnumerable<Result> secondResults,
                                                          Func<IEnumerable<double>, IEnumerable<double>, double> correlationFunction)
        {
            var firstResultsFrequencies = firstResults.Select(r => r.Frequency);
            var secondResultsFrequencies = secondResults.Select(r => r.Frequency);

            return correlationFunction(firstResultsFrequencies, secondResultsFrequencies);
        }
    }
}
