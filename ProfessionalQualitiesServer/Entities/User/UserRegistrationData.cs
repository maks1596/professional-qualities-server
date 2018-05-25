using System;
using System.Linq;

namespace ProfessionalQualitiesServer.Entities
{
    public class UserRegistrationData
    {
        public UserRegistrationData() { }

        public string Login { get; set; }
        public string Password { get; set; }
        public string Name { get; set; }
        public bool IsProgrammer { get; set; }

        public UserEntity ToUserEntity(ProfessionalQualitiesDbContext dbContext)
        {
            var userEntity = new UserEntity
            {
                Login = Login,
                Password = Password,
                PersonalData = new PersonalDataEntity
                {
                    Name = Name,
                    IsMale = true,
                    Birthday = DateTime.Today
                },
                Role = dbContext.Roles
                                .Single(re => re.Name == Constants.UserRoleString)
                
            };

            if (IsProgrammer)
            {
                userEntity.PersonalData.Profession = dbContext.Professions
                    .Single(pe => pe.Name == Constants.ProgrammerProfessionString);
            }

            return userEntity;
        }
    }
}
