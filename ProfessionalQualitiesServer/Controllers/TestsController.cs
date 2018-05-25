using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProfessionalQualitiesServer.Entities;

namespace ProfessionalQualitiesServer.Controllers
{
    [Route("api/[controller]")]
    public class TestsController : Controller
    {
        public TestsController(ProfessionalQualitiesDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        private ProfessionalQualitiesDbContext _dbContext;

        // GET api/tests
        [HttpGet]
        public IEnumerable<ShortTestInfo> Get()
        {
            return _dbContext.Tests
                    .OrderBy(testEntity => testEntity.Name)
                    .Select(test => new ShortTestInfo(test.Id, test.Name));
        }

        // GET api/tests/5
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            if (_dbContext.Tests.Any(te => te.Id == id))
            {
                var testEntity = _dbContext.Tests
                                 .Include(te => te.TestAnswerOptions)
                                    .ThenInclude(tao => tao.AnswerOption)
                                 .Include(te => te.Questions)
                                    .ThenInclude(qe => qe.QuestionAnswerOptions)
                                        .ThenInclude(qao => qao.AnswerOption)
                                 .Single(te => te.Id == id);

                return Ok(new Test(testEntity));
            }
            else
            {
                return NotFound();
            }
            
        }

        // GET api/tests/short/5
        [HttpGet("short/{id}")]
        public IActionResult GetShortTestInfo(int id)
        {
            if (_dbContext.Tests.Any(te => te.Id == id))
            {
                var testEntity = _dbContext.Tests
                                 .Single(te => te.Id == id);
                return Ok(new ShortTestInfo(testEntity));
            }
            else
            {
                return NotFound();
            }
        }

        // GET api/tests/short/{name}
        [HttpGet("short/by-name/{name}")]
        public IActionResult GetShortTestInfo(string name)
        {
            name = name.Replace('_', ' ');

            if (_dbContext.Tests.Any(te => te.Name == name))
            {
                var testEntity = _dbContext.Tests
                                 .Single(te => te.Name == name);
                return Ok(new ShortTestInfo(testEntity));
            }
            else
            {
                return NotFound();
            }
        }

        // GET api/tests/with-scales/5
        [HttpGet("with-scales/{id}")]
        public IActionResult GetWithScales(int id)
        {
            if (_dbContext.Tests.Any(te => te.Id == id))
            {
                var testEntity = _dbContext.Tests
                                 .Include(te => te.TestAnswerOptions)
                                    .ThenInclude(tao => tao.AnswerOption)
                                 .Include(te => te.Questions)
                                    .ThenInclude(qe => qe.QuestionAnswerOptions)
                                        .ThenInclude(qao => qao.AnswerOption)
                                 .Include(te => te.Key)
                                    .ThenInclude(ke => ke.Scale)
                                 .Include(te => te.EvaluationMap)
                                    .ThenInclude(ke => ke.Scale)
                                 .Single(te => te.Id == id);
                return Ok(new TestWithScales(testEntity));
            }
            else
            {
                return NotFound();
            }
        }

        // GET api/tests/for-user/5
        [HttpGet("for-user/{userId}")]
        public IActionResult GetTestsForUser(int userId)
        {
            if (_dbContext.Users.Any(ue => ue.Id == userId))
            {
                var testEntitites = _dbContext.Tests
                    .Include(te => te.PassedTests)
                    .OrderBy(te => te.Name);

                foreach (var testEntity in testEntitites)
                {
                    if (testEntity.PassedTests != null && testEntity.PassedTests.Count > 0)
                    {
                        testEntity.PassedTests = testEntity.PassedTests
                            .Where(pte => pte.TestedId == userId)
                            .ToList();
                    }
                }

                var testsWithStatus = testEntitites
                    .Select(te => new TestWithStatus(te));
                return Ok(testsWithStatus);
            }
            else
            {
                return NotFound();
            }
        }

        // PUT api/tests
        [HttpPut]
        public IActionResult Put([FromBody] TestWithScales test)
        {
            if (_dbContext.Tests.Any(te => te.Name == test.Name))
            {
                return new StatusCodeResult(409);
            }

            var testEntity = test.ToEntity(_dbContext);
            _dbContext.Tests.Add(testEntity);
            _dbContext.SaveChanges();
            return Ok();
        }

        // PUT api/tests
        [HttpPatch]
        public IActionResult Patch([FromBody] TestWithScales test)
        {
            return NotFound();            
        }

        // DELETE api/tests/5
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            if (_dbContext.Tests.Any(te => te.Id == id))
            {
                var testEntity = _dbContext.Tests
                    .Include(te => te.PassedTests)
                    .Single(te => te.Id == id);
                if (testEntity.PassedTests != null && testEntity.PassedTests.Count > 0)
                {
                    return Forbid();
                }
                _dbContext.Tests.Remove(testEntity);
                _dbContext.SaveChanges();
                return Ok();
            }
            else
            {
                return NotFound();
            }
        }

        // POST api/tests/count-results
        [HttpPost("count-results")]
        public IActionResult CountResults([FromBody] BindedAnswers bindedAnswers)
        {
            if (!_dbContext.Users.Any(ue => ue.Id == bindedAnswers.UserId) || 
                !_dbContext.Tests.Any(te => te.Id == bindedAnswers.TestId))
            {
                return NotFound();
            }

            var testEntity = _dbContext.Tests
                .Include(te => te.Key)
                    .ThenInclude(ke => ke.Scale)
                .Include(te => te.EvaluationMap)
                    .ThenInclude(eme => eme.Scale)
                .Single(te => te.Id == bindedAnswers.TestId);

            var evaluatedScales = CountPoints(bindedAnswers.Answers, testEntity.Key);
            var results = GetResults(evaluatedScales, testEntity.EvaluationMap);

            int passedTestId = SaveAnswers(bindedAnswers.UserId, bindedAnswers.TestId, bindedAnswers.Answers);
            SaveResults(passedTestId, results);

            return Ok(results);
        }

        private IEnumerable<EvaluatedScale> CountPoints(IEnumerable<Answer> answers, IEnumerable<KeyEntity> key)
        {
            var keyScales = key.GroupBy(ke => ke.ScaleId);

            foreach (var group in keyScales)
            {
                var evaluatedScale = new EvaluatedScale
                {
                    ScaleId = group.Key,
                    Points = 0
                };

                foreach (var evaluatedAnswer in group)
                {
                    if (answers.Any(answer => answer.AnswerId == evaluatedAnswer.AnswerId &&
                                              answer.QuestionId == evaluatedAnswer.QuestionId))
                    {
                        evaluatedScale.Points += evaluatedAnswer.Points;
                    }
                }
                yield return evaluatedScale;
            }
        }

        private IEnumerable<ScaleResult> GetResults(IEnumerable<EvaluatedScale> evaluatedScales, 
                                                    IEnumerable<EvaluationMapEntity> evaluationMap)
        {
            foreach (var evaluatedScale in evaluatedScales)
            {
                Func<EvaluationMapEntity, bool> conditionLambda = eme => eme.ScaleId == evaluatedScale.ScaleId &&
                                                                         eme.LowerRangeValue <= evaluatedScale.Points &&
                                                                         eme.UpperRangeValue >= evaluatedScale.Points;

                if (evaluationMap.Any(conditionLambda))
                {
                    var evaluationMapEntity = evaluationMap.Single(conditionLambda);
                    yield return new ScaleResult(evaluationMapEntity, evaluatedScale.Points);
                }
            }
        }

        private int SaveAnswers(int userId, int testId, IEnumerable<Answer> answers)
        {
            var passedTestEntity = new PassedTestEntity
            {
                TestedId = userId,
                TestId = testId,
                Date = DateTime.Now
            };
            _dbContext.Add(passedTestEntity);

            foreach (var answer in answers)
            {
                var answerEntity = new AnswerEntity
                {
                    PassedTestId = passedTestEntity.Id,
                    QuestionId = answer.QuestionId,
                    AnswerId = answer.AnswerId
                };
                _dbContext.Add(answerEntity);
            }
            _dbContext.SaveChanges();
            return passedTestEntity.Id;
        }

        private void SaveResults(int passedTestId, IEnumerable<ScaleResult> results)
        {
            foreach (var scaleResult in results)
            {
                var resultEntity = new ResultEntity
                {
                    PassedTestId = passedTestId,
                    ScaleId = scaleResult.ScaleId,
                    Points = scaleResult.Points,
                    Result = scaleResult.Result
                };
            }
        }
    }
}