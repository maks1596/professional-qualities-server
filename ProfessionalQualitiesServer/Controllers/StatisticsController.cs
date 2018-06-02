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
                             .OrderBy(te => te.Name)
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

        // GET: api/statistics/covariance?testId=1&firstScaleId=1&secondScaleId=2
        [HttpGet("covariance")]
        public IActionResult GetCovariance(int testId, int firstScaleId, int secondScaleId)
        {
            if (!_dbContext.PassedTests.Any(pte => pte.TestId == testId))
            {
                return NotFound();
            }

            var testEntity = GetTestEntity(testId);
            return Ok(new { value = Entities.Statistics.Scale.CountCovariance(testEntity, firstScaleId, secondScaleId) });
        }

        // GET: api/statistics/pearson?testId=1&firstScaleId=1&secondScaleId=2
        [HttpGet("pearson")]
        public IActionResult GetPearson(int testId, int firstScaleId, int secondScaleId)
        {
            if (!_dbContext.PassedTests.Any(pte => pte.TestId == testId))
            {
                return NotFound();
            }

            var testEntity = GetTestEntity(testId);
            return Ok(new { value = Entities.Statistics.Scale.CountPearsonCorrelationCoefficient(testEntity, firstScaleId, secondScaleId) });
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
