using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProfessionalQualitiesServer.Entities.Statistics;

namespace ProfessionalQualitiesServer.Controllers
{
    [Route("api/[controller]")]
    public class StatisticsController : Controller
    {
        public StatisticsController(ProfessionalQualitiesDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        private ProfessionalQualitiesDbContext _dbContext;

        // GET: api/statistics
        [HttpGet]
        public IEnumerable<PassedTestPreview> Get()
        {
            return _dbContext.Tests
                             .Include(te => te.PassedTests)
                             .Select(te => new PassedTestPreview(te));
        }

        // GET: api/statistics/testId
        [HttpGet("{testId}")]
        public IActionResult Get(int testId)
        {
            if (!_dbContext.PassedTests.Any(pte => pte.TestId == testId))
            {
                return NotFound();
            }

            var testEntity = GetTestEntity(testId);
            return Ok(new Entities.Statistics.Test(testEntity));
        }

        // GET: api/statistics/correlations/testId/scaleId
        [HttpGet("correlations/{testId}/{scaleId}")]
        public IActionResult GetCorrelations(int testId, int scaleId)
        {
            if (!_dbContext.PassedTests.Any(pte => pte.TestId == testId))
            {
                return NotFound(testId);
            }

            var testEntity = GetTestEntity(testId);
            if (!testEntity.GetScales().Any(se => se.Id == scaleId))
            {
                return NotFound(scaleId);
            }

            return Ok(MakeDefaultGroupsCorrelations(testEntity, scaleId));
        }

        // GET: api/statistics/correlations/testId/scaleId/professionId
        [HttpGet("correlations/{testId}/{scaleId}/{professionId}")]
        public IActionResult GetCorrelations(int testId, int scaleId, int professionId)
        {
            if (!_dbContext.PassedTests.Any(pte => pte.TestId == testId))
            {
                return NotFound();
            }

            var testEntity = GetTestEntity(testId);
        }

        private IEnumerable<GroupCorrelations> MakeDefaultGroupsCorrelations(TestEntity testEntity, int scaleId)
        {
            var programmerProfessionId = _dbContext.Professions
                                                   .Single(pe => pe.Name == Constants.ProgrammerProfessionString)
                                                   .Id;
            yield return new GroupCorrelations(testEntity, scaleId, Constants.EveryoneGroupNameString);
            yield return new GroupCorrelations(testEntity, scaleId, programmerProfessionId, Constants.ProgrammersGroupNameString);
            yield return new GroupCorrelations(testEntity, scaleId, -programmerProfessionId, Constants.NonProgrammersGroupNameString);
        }

        private TestEntity GetTestEntity(int testId)
        {
            return _dbContext.Tests
                             .Include(te => te.PassedTests)
                                .ThenInclude(pte => pte.Results)
                            .Include(te => te.PassedTests)
                                .ThenInclude(pte => pte.Tested)
                                    .ThenInclude(te => te.PersonalData)
                                        .ThenInclude(pde => pde.Profession)
                            .Include(te => te.EvaluationMap)
                                .ThenInclude(ke => ke.Scale)
                            .Include(te => te.Key)
                                .ThenInclude(ke => ke.Scale)
                            .Single(te => te.Id == testId);
        }
    }
}
