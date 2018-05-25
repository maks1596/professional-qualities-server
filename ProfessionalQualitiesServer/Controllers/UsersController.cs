﻿using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ProfessionalQualitiesServer.Controllers
{
    [Route("api/[controller]")]
    public class UsersController : Controller
    {
        public UsersController(ProfessionalQualitiesDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        private ProfessionalQualitiesDbContext _dbContext;

        // GET: api/users
        [HttpGet]
        public IEnumerable<User> Get()
        {
            return _dbContext.Users
                    .Include(userEntity => userEntity.PersonalData)
                        .ThenInclude(personalDataEntity => personalDataEntity.Profession)
                    .Where(userEntity => !userEntity.Deleted)
                    .OrderBy(userEntity => userEntity.PersonalData.Name)
                    .Select(userEntity => new User(userEntity));
        }

        // GET: api/users/5
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            if (_dbContext.Users.Any(ue => ue.Id == id))
            {
                var user = new User(_dbContext.Users
                                    .Include(userEntity => userEntity.PersonalData)
                                        .ThenInclude(personalDataEntity => personalDataEntity.Profession)
                                    .Single(userEntity => userEntity.Id == id));
                return Ok(user);
            }
            else
            {
                return NotFound();
            }

        }

        // GET: api/users/professions
        [HttpGet("professions")]
        public IEnumerable<string> GetProfessions()
        {
            return _dbContext.Professions
                .Select(professionEntity => professionEntity.Name);
        }

        // PUT: api/users
        [HttpPut]
        public IActionResult Put([FromBody]User user)
        {
            bool loginIsUsed = _dbContext.Users
                               .Any(userEntity => userEntity.Login == user.Login);
            if (loginIsUsed)
            {
                return new StatusCodeResult(409);
            }

            _dbContext.Users.Add(user.ToEntity(_dbContext));
            _dbContext.SaveChanges();
            return Ok();
        }

        // PUT: api/users
        [HttpPatch]
        public IActionResult Patch([FromBody] User user) // TODO: Заменить User на PersonalData
        {
            if (_dbContext.Users.Any(ue => ue.Id == user.Id))
            {
                var userEntity = _dbContext.Users
                                .Include(ue => ue.PersonalData)
                                    .ThenInclude(pde => pde.Profession)
                                .Single(ue => ue.Id == user.Id);
                var newPersonalData = user.ToEntity(_dbContext).PersonalData;
                if (userEntity.PersonalData == null)
                {
                    userEntity.PersonalData = new PersonalDataEntity();
                }
                userEntity.PersonalData.Name = newPersonalData.Name;
                userEntity.PersonalData.IsMale = newPersonalData.IsMale;
                userEntity.PersonalData.Birthday = newPersonalData.Birthday;
                userEntity.PersonalData.Profession = newPersonalData.Profession;
                userEntity.PersonalData.ExpertAssessment = newPersonalData.ExpertAssessment;

                _dbContext.SaveChanges();
                return Ok();
            }
            else
            {
                return NotFound();
            }
            
        }

        // DELETE: api/users/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            var userEntity = _dbContext.Users.Single(ue => ue.Id == id);
            if (userEntity.PassedTests != null && userEntity.PassedTests.Count > 0)
            {
                userEntity.Deleted = true;
            }
            else
            {
                _dbContext.Users.Remove(userEntity);
            }
            _dbContext.SaveChanges();
        }
    }
}
