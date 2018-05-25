using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProfessionalQualitiesServer
{
    public class User
    {
        

        public User() : this(0) { }

        public User(int id = 0)
        {
            Id = id;
            Gender = Constants.DefaultGenderString;
            RoleName = Constants.DefaultRoleString;
            Birthday = DateTime.Today;
            ExpertAssessment = -1;
        }

        public User(UserEntity userEntity)
        {
            Id = userEntity.Id;
            Login = userEntity.Login;
            Password = userEntity.Password;

            if (userEntity.Role != null)
            {
                RoleName = userEntity.Role.Name;
            }

            if (userEntity.PersonalData != null)
            {
                var personalData = userEntity.PersonalData;

                Name = personalData.Name;
                Gender = personalData.IsMale ? Constants.MaleGenderString : Constants.FemaleGenderString;
                Birthday = personalData.Birthday;
                ExpertAssessment = personalData.ExpertAssessment;



                if (personalData.Profession != null)
                {
                    ProfessionName = personalData.Profession.Name;
                }
            }
        }

        public int Id { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public string RoleName { get; set; }

        public string Name { get; set; }
        public string Gender { get; set; }
        [JsonConverter(typeof(IsoDateConverter))]
        public DateTime Birthday { get; set; }
        public string ProfessionName { get; set; }
        public int ExpertAssessment { get; set; }

        public UserEntity ToEntity(ProfessionalQualitiesDbContext dbContext)
        {
            var entity = new UserEntity
            {
                Id = Id,
                Login = Login,
                Password = Password,
                Role = GetRoleEntity(dbContext),
                Deleted = false,

                PersonalData = new PersonalDataEntity
                {
                    Name = Name,
                    IsMale = (Gender == Constants.MaleGenderString),
                    Birthday = Birthday,
                    ExpertAssessment = ExpertAssessment
                }
            };

            if (ProfessionName != null && ProfessionName.Length > 0)
            {
                entity.PersonalData.Profession = GetProfessionEntity(dbContext);
            }
            else
            {
                entity.PersonalData.Profession = null;
            }

            return entity;
        }

        private ProfessionEntity GetProfessionEntity(ProfessionalQualitiesDbContext dbContext)
        {
            var professionEntity = dbContext.Professions
                                            .SingleOrDefault(pe => pe.Name == ProfessionName);
            if (professionEntity == null)
            {
                professionEntity = new ProfessionEntity
                {
                    Name = ProfessionName
                };
                dbContext.Professions.Add(professionEntity);
                dbContext.SaveChanges();
            }

            return professionEntity;
        }

        private RoleEntity GetRoleEntity(ProfessionalQualitiesDbContext dbContext)
        {
            if (!dbContext.Roles.Any(re => re.Name == RoleName))
            {
                RoleName = Constants.DefaultRoleString;
            }
            return dbContext.Roles.Single(re => re.Name == RoleName);
        }
    }
}
