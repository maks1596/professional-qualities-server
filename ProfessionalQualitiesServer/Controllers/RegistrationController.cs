using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProfessionalQualitiesServer.Entities;

namespace ProfessionalQualitiesServer.Controllers
{
    [Route("api/[controller]")]
    public class RegistrationController : Controller
    {
        public RegistrationController(ProfessionalQualitiesDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        private ProfessionalQualitiesDbContext _dbContext;

        // POST: api/registration
        [HttpPost]
        public IActionResult Post([FromBody]UserRegistrationData userRegistrationData)
        {
            if (_dbContext.Users.Any(ue => ue.Login == userRegistrationData.Login))
            {
                return new StatusCodeResult(409);
            }

            var userEntity = userRegistrationData.ToUserEntity(_dbContext);
            _dbContext.Users.Add(userEntity);
            _dbContext.SaveChanges();
            return Ok(new { userId = userEntity.Id });
        }
    }
}
