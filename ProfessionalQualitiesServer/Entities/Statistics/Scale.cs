using System;
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


            var allResultEntities = testEntity.GetResults()
                                       .Where(re => re.ScaleId == scaleEntity.Id);
            InitGroupsResults(allResultEntities);
            
        }


        public int Id { get; set; }
        public string Name { get; set; }
        public List<GroupResults> GroupsResults { get; set; }

        private void InitGroupsResults(IEnumerable<ResultEntity> allResultEntities)
        {
            var programmerGroupResults = new GroupResults(Constants.ProgrammersGroupNameString);
            var nonProgrammerGroupResults = new GroupResults(Constants.NonProgrammersGroupNameString);
            var groupedResultEntities = allResultEntities.GroupBy(IsProgrammer);

            foreach (var group in groupedResultEntities)
            {
                if (group.Key)  // Программист
                {
                    programmerGroupResults.Results = GroupResults.MakeResults(group);
                }
                else            // Не программист
                {
                    nonProgrammerGroupResults.Results = GroupResults.MakeResults(group);
                }
            }

            GroupsResults = new List<GroupResults>();
            GroupsResults.Add(new GroupResults(Constants.EveryoneGroupNameString, allResultEntities));
            GroupsResults.Add(programmerGroupResults);
            GroupsResults.Add(nonProgrammerGroupResults);
        }

        private static IEnumerable<Result> MakeResultsForScale(IEnumerable<ResultEntity> resultEntities, int scaleId)
        {
            return GroupResults.MakeResults(resultEntities.Where(re => re.ScaleId == scaleId));
        }

        private bool IsProgrammer(ResultEntity re)
        {
            var personalData = re.PassedTest.Tested.PersonalData;
            if (personalData != null)
            {
                return personalData.Profession.Name == Constants.ProgrammerProfessionString;
            }
            return false;
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
