using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System;
using System.Linq;
using System.ComponentModel.DataAnnotations;

namespace ProfessionalQualitiesServer
{
    public class ProfessionalQualitiesDbContext : DbContext
    {
        public ProfessionalQualitiesDbContext(DbContextOptions<ProfessionalQualitiesDbContext> options) : base(options) { }

        public DbSet<RoleEntity> Roles { get; set; }
        public DbSet<UserEntity> Users { get; set; }
        public DbSet<ProfessionEntity> Professions { get; set; }

        public DbSet<TestEntity> Tests { get; set; }
        public DbSet<QuestionEntity> Questions { get; set; }
        public DbSet<AnswerOptionEntity> AnswerOptions { get; set; }

        public DbSet<TestAnswerOption> TestsAnswerOptions { get; set; }
        public DbSet<QuestionAnswerOption> QuestionsAnswerOptions { get; set; }

        public DbSet<ScaleEntity> Scales { get; set; }
        public DbSet<KeyEntity> Keys { get; set; }
        public DbSet<EvaluationMapEntity> EvaluationMaps { get; set; }

        public DbSet<PassedTestEntity> PassedTests { get; set; }
        public DbSet<AnswerEntity> Answers { get; set; }
        public DbSet<ResultEntity> Results { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // User and PersonalData
            modelBuilder.Entity<UserEntity>()
                .HasOne(ue => ue.PersonalData)
                .WithOne(pde => pde.User)
                .OnDelete(DeleteBehavior.Cascade);

            // Test and Questions
            modelBuilder.Entity<TestEntity>()
                .HasMany(te => te.Questions)
                .WithOne(qe => qe.Test)
                .OnDelete(DeleteBehavior.Cascade);

            //  Test and AnswerOptions
            modelBuilder.Entity<TestAnswerOption>()
               .HasKey(ta => new { ta.TestId, ta.AnswerOptionId });

            modelBuilder.Entity<TestEntity>()
                .HasMany(te => te.TestAnswerOptions)
                .WithOne(tao => tao.Test)
                .OnDelete(DeleteBehavior.Cascade);

            // Question and AsnwerOptions
            modelBuilder.Entity<QuestionAnswerOption>()
                .HasKey(qa => new { qa.QuestionId, qa.AnswerOptionId });

            modelBuilder.Entity<QuestionEntity>()
                .HasMany(qe => qe.QuestionAnswerOptions)
                .WithOne(qao => qao.Question)
                .OnDelete(DeleteBehavior.Cascade);

            // Key
            modelBuilder.Entity<KeyEntity>()
                .HasKey(ke => new { ke.TestId, ke.ScaleId, ke.QuestionId, ke.AnswerId });

            // Test and Key
            modelBuilder.Entity<TestEntity>()
                .HasMany(te => te.Key)
                .WithOne(ke => ke.Test)
                .OnDelete(DeleteBehavior.Cascade);

            // EvaluationMap
            modelBuilder.Entity<EvaluationMapEntity>()
                .HasKey(eme => new { eme.TestId, eme.ScaleId, eme.LowerRangeValue, eme.UpperRangeValue });

            // Test and EvaluationMap
            modelBuilder.Entity<TestEntity>()
                .HasMany(te => te.EvaluationMap)
                .WithOne(eme => eme.Test)
                .OnDelete(DeleteBehavior.Cascade);

            // PassedTest and AnswerEntity
            modelBuilder.Entity<PassedTestEntity>()
                .HasMany(pte => pte.Answers)
                .WithOne(ae => ae.PassedTest)
                .OnDelete(DeleteBehavior.Cascade);

            // Result
            modelBuilder.Entity<ResultEntity>()
                .HasKey(re => new { re.PassedTestId, re.ScaleId });

            // PassedTest and Result
            modelBuilder.Entity<PassedTestEntity>()
                .HasMany(pte => pte.Results)
                .WithOne(re => re.PassedTest)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }

    //  :: Пользователь ::

    public class RoleEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public List<UserEntity> Users { get; set; }
    }

    public class UserEntity
    {
        public int Id { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public RoleEntity Role { get; set; }
        public PersonalDataEntity PersonalData { get; set; }
        public bool Deleted { get; set; }

        public List<PassedTestEntity> PassedTests { get; set; }
    }

    public class ProfessionEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public List<PersonalDataEntity> PersonalDataOfUsers { get; set; }
    }

    public class PersonalDataEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool IsMale { get; set; }
        public DateTime Birthday { get; set; }
        public int ExpertAssessment { get; set; }

        public int UserId { get; set; }
        public UserEntity User { get; set; }

        public int ProfessionId { get; set; }
        public ProfessionEntity Profession { get; set; }
    }

    //  :: Тест ::

    public class TestEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Instruction { get; set; }
        public int AmountOfVisibleQuestions { get; set; }
        public bool AreAnswerOptionsUnique { get; set; }

        public List<TestAnswerOption> TestAnswerOptions { get; set; }

        public List<QuestionEntity> Questions { get; set; }
        
        public List<EvaluationMapEntity> EvaluationMap { get; set; }
        public List<KeyEntity> Key { get; set; }

        public List<ScaleEntity> GetScales()
        { 
            return EvaluationMap.Select(eme => eme.Scale)
                                .Union(Key.Select(ke => ke.Scale)
                                )
                                .Distinct()
                                .ToList();
        }

        public List<PassedTestEntity> PassedTests { get; set; }

        public List<ResultEntity> GetResults()
        {
           return PassedTests != null ? PassedTests.SelectMany(pte => pte.Results).ToList() : null;
        }
    }

    public class QuestionEntity
    {
        public int Id { get; set; }
        public TestEntity Test { get; set; }

        [Required]
        public int Order { get; set; }
        public string Formulation { get; set; }

        public List<QuestionAnswerOption> QuestionAnswerOptions { get; set; }
    }

    public class AnswerOptionEntity
    {
        public int Id { get; set; }
        public string Formulation { get; set; }
    }

    public class TestAnswerOption
    {
        public int TestId { get; set; }
        public TestEntity Test { get; set; }

        public int AnswerOptionId { get; set; }
        public AnswerOptionEntity AnswerOption { get; set; }

        [Required]
        public int Order { get; set; }
    }

    public class QuestionAnswerOption
    {
        public int QuestionId { get; set; }
        public QuestionEntity Question { get; set; }

        public int AnswerOptionId { get; set; }
        public AnswerOptionEntity AnswerOption { get; set; }

        [Required]
        public int Order { get; set; }
    }

    //  :: Оценивание теста ::

    public class ScaleEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

    public class KeyEntity
    {
        public int TestId { get; set; }
        public TestEntity Test { get; set; }

        public int ScaleId { get; set; }
        public ScaleEntity Scale { get; set; }

        public int AnswerId { get; set; }
        public AnswerOptionEntity Answer { get; set; }

        public int QuestionId { get; set; }
        public QuestionEntity Question { get; set; }

        public int Points { get; set; }
    }

    public class EvaluationMapEntity
    {
        public int TestId { get; set; }
        public TestEntity Test { get; set; }

        public int ScaleId { get; set; }
        public ScaleEntity Scale { get; set; }

        public int LowerRangeValue { get; set; }
        public int UpperRangeValue { get; set; }
        public string Result { get; set; }
    }

    //  :: Пройденный тест ::

    public class PassedTestEntity
    {
        public int Id { get; set; }

        public int TestId { get; set; }
        public TestEntity Test { get; set; }

        public int TestedId { get; set; }
        public UserEntity Tested { get; set; }

        public int ProfessionId { get; set; }
        public ProfessionEntity Profession { get; set; }
        public int ExpertAsessment { get; set; }

        public DateTime Date { get; set; }

        public List<AnswerEntity> Answers { get; set; }
        public List<ResultEntity> Results { get; set; }
    }

    public class AnswerEntity
    {
        public int Id { get; set; }

        public int PassedTestId { get; set; }
        public PassedTestEntity PassedTest { get; set; }

        public int QuestionId { get; set; }
        public QuestionEntity Question { get; set; }

        public int AnswerId { get; set; }
        public AnswerOptionEntity Answer { get; set; }
    }

    public class ResultEntity
    {
        public int PassedTestId { get; set; }
        public PassedTestEntity PassedTest { get; set; }

        public int ScaleId { get; set; }
        public ScaleEntity Scale { get; set; }

        public int Points { get; set; }
        public string Result { get; set; }
    }
}