using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProfessionalQualitiesServer.Entities;

namespace ProfessionalQualitiesServer.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    public class AutorizationController : Controller
    {
        public AutorizationController(ProfessionalQualitiesDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        private ProfessionalQualitiesDbContext _dbContext;

        // POST: api/autorization
        [HttpPost]
        public IActionResult AutorizeAsUser([FromBody]AutorizationData data)
        {
            if (_dbContext.Users
                .Any(ue => ue.Login == data.Login && 
                           ue.Password == data.Password &&
                           !ue.Deleted))
            {
                int userId = _dbContext.Users
                    .Single(ue => ue.Login == data.Login &&
                                  ue.Password == data.Password &&
                                  !ue.Deleted)
                     .Id;
                return Ok(new { userId });
            }
            else
            {
                return NotFound();
            }
        }

        // POST: api/autorization/admin
        [HttpPost("admin")]
        public IActionResult AutorizeAsAdmin([FromBody]AutorizationData data)
        {
            if (_dbContext.Users
                .Any(ue => ue.Login == data.Login &&
                           ue.Password == data.Password &&
                           !ue.Deleted &&
                           ue.Role.Name == Constants.AdminRoleString))
            {
                return Ok();
            }
            else
            {
                return NotFound();
            }
        }
    }
}
