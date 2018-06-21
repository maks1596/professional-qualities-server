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
                                              .Where(re => re.ScaleId == scaleEntity.Id)
                                              .OrderByDescending(re => re.Points);
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
            var professionEntity = re.PassedTest.Profession;
            if (professionEntity != null)
            {
                return professionEntity.Name == Constants.ProgrammerProfessionString;
            }
            return false;
        }
    }
}
