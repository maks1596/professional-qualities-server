﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Storage.Internal;
using ProfessionalQualitiesServer;
using System;

namespace ProfessionalQualitiesServer.Migrations
{
    [DbContext(typeof(ProfessionalQualitiesDbContext))]
    [Migration("20180603194328_legendary_migration")]
    partial class legendary_migration
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn)
                .HasAnnotation("ProductVersion", "2.0.1-rtm-125");

            modelBuilder.Entity("ProfessionalQualitiesServer.AnswerEntity", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("AnswerId");

                    b.Property<int>("PassedTestId");

                    b.Property<int>("QuestionId");

                    b.HasKey("Id");

                    b.HasIndex("AnswerId");

                    b.HasIndex("PassedTestId");

                    b.HasIndex("QuestionId");

                    b.ToTable("Answers");
                });

            modelBuilder.Entity("ProfessionalQualitiesServer.AnswerOptionEntity", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Formulation");

                    b.HasKey("Id");

                    b.ToTable("AnswerOptions");
                });

            modelBuilder.Entity("ProfessionalQualitiesServer.EvaluationMapEntity", b =>
                {
                    b.Property<int>("TestId");

                    b.Property<int>("ScaleId");

                    b.Property<int>("LowerRangeValue");

                    b.Property<int>("UpperRangeValue");

                    b.Property<string>("Result");

                    b.HasKey("TestId", "ScaleId", "LowerRangeValue", "UpperRangeValue");

                    b.HasIndex("ScaleId");

                    b.ToTable("EvaluationMaps");
                });

            modelBuilder.Entity("ProfessionalQualitiesServer.KeyEntity", b =>
                {
                    b.Property<int>("TestId");

                    b.Property<int>("ScaleId");

                    b.Property<int>("QuestionId");

                    b.Property<int>("AnswerId");

                    b.Property<int>("Points");

                    b.HasKey("TestId", "ScaleId", "QuestionId", "AnswerId");

                    b.HasIndex("AnswerId");

                    b.HasIndex("QuestionId");

                    b.HasIndex("ScaleId");

                    b.ToTable("Keys");
                });

            modelBuilder.Entity("ProfessionalQualitiesServer.PassedTestEntity", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("Date");

                    b.Property<int>("TestId");

                    b.Property<int>("TestedId");

                    b.HasKey("Id");

                    b.HasIndex("TestId");

                    b.HasIndex("TestedId");

                    b.ToTable("PassedTests");
                });

            modelBuilder.Entity("ProfessionalQualitiesServer.PersonalDataEntity", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("Birthday");

                    b.Property<int>("ExpertAssessment");

                    b.Property<bool>("IsMale");

                    b.Property<string>("Name");

                    b.Property<int>("ProfessionId");

                    b.Property<int>("UserId");

                    b.HasKey("Id");

                    b.HasIndex("ProfessionId");

                    b.HasIndex("UserId")
                        .IsUnique();

                    b.ToTable("PersonalDataEntity");
                });

            modelBuilder.Entity("ProfessionalQualitiesServer.ProfessionEntity", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Name");

                    b.HasKey("Id");

                    b.ToTable("Professions");
                });

            modelBuilder.Entity("ProfessionalQualitiesServer.QuestionAnswerOption", b =>
                {
                    b.Property<int>("QuestionId");

                    b.Property<int>("AnswerOptionId");

                    b.Property<int>("Order");

                    b.HasKey("QuestionId", "AnswerOptionId");

                    b.HasIndex("AnswerOptionId");

                    b.ToTable("QuestionsAnswerOptions");
                });

            modelBuilder.Entity("ProfessionalQualitiesServer.QuestionEntity", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Formulation");

                    b.Property<int>("Order");

                    b.Property<int?>("TestId");

                    b.HasKey("Id");

                    b.HasIndex("TestId");

                    b.ToTable("Questions");
                });

            modelBuilder.Entity("ProfessionalQualitiesServer.ResultEntity", b =>
                {
                    b.Property<int>("PassedTestId");

                    b.Property<int>("ScaleId");

                    b.Property<int>("Points");

                    b.Property<string>("Result");

                    b.HasKey("PassedTestId", "ScaleId");

                    b.HasIndex("ScaleId");

                    b.ToTable("Results");
                });

            modelBuilder.Entity("ProfessionalQualitiesServer.RoleEntity", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Name");

                    b.HasKey("Id");

                    b.ToTable("Roles");
                });

            modelBuilder.Entity("ProfessionalQualitiesServer.ScaleEntity", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Name");

                    b.HasKey("Id");

                    b.ToTable("Scales");
                });

            modelBuilder.Entity("ProfessionalQualitiesServer.TestAnswerOption", b =>
                {
                    b.Property<int>("TestId");

                    b.Property<int>("AnswerOptionId");

                    b.Property<int>("Order");

                    b.HasKey("TestId", "AnswerOptionId");

                    b.HasIndex("AnswerOptionId");

                    b.ToTable("TestsAnswerOptions");
                });

            modelBuilder.Entity("ProfessionalQualitiesServer.TestEntity", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("AmountOfVisibleQuestions");

                    b.Property<bool>("AreAnswerOptionsUnique");

                    b.Property<string>("Instruction");

                    b.Property<string>("Name");

                    b.HasKey("Id");

                    b.ToTable("Tests");
                });

            modelBuilder.Entity("ProfessionalQualitiesServer.UserEntity", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<bool>("Deleted");

                    b.Property<string>("Login");

                    b.Property<string>("Password");

                    b.Property<int?>("RoleId");

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("ProfessionalQualitiesServer.AnswerEntity", b =>
                {
                    b.HasOne("ProfessionalQualitiesServer.AnswerOptionEntity", "Answer")
                        .WithMany()
                        .HasForeignKey("AnswerId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("ProfessionalQualitiesServer.PassedTestEntity", "PassedTest")
                        .WithMany("Answers")
                        .HasForeignKey("PassedTestId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("ProfessionalQualitiesServer.QuestionEntity", "Question")
                        .WithMany()
                        .HasForeignKey("QuestionId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("ProfessionalQualitiesServer.EvaluationMapEntity", b =>
                {
                    b.HasOne("ProfessionalQualitiesServer.ScaleEntity", "Scale")
                        .WithMany()
                        .HasForeignKey("ScaleId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("ProfessionalQualitiesServer.TestEntity", "Test")
                        .WithMany("EvaluationMap")
                        .HasForeignKey("TestId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("ProfessionalQualitiesServer.KeyEntity", b =>
                {
                    b.HasOne("ProfessionalQualitiesServer.AnswerOptionEntity", "Answer")
                        .WithMany()
                        .HasForeignKey("AnswerId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("ProfessionalQualitiesServer.QuestionEntity", "Question")
                        .WithMany()
                        .HasForeignKey("QuestionId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("ProfessionalQualitiesServer.ScaleEntity", "Scale")
                        .WithMany()
                        .HasForeignKey("ScaleId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("ProfessionalQualitiesServer.TestEntity", "Test")
                        .WithMany("Key")
                        .HasForeignKey("TestId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("ProfessionalQualitiesServer.PassedTestEntity", b =>
                {
                    b.HasOne("ProfessionalQualitiesServer.TestEntity", "Test")
                        .WithMany("PassedTests")
                        .HasForeignKey("TestId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("ProfessionalQualitiesServer.UserEntity", "Tested")
                        .WithMany("PassedTests")
                        .HasForeignKey("TestedId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("ProfessionalQualitiesServer.PersonalDataEntity", b =>
                {
                    b.HasOne("ProfessionalQualitiesServer.ProfessionEntity", "Profession")
                        .WithMany("PersonalDataOfUsers")
                        .HasForeignKey("ProfessionId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("ProfessionalQualitiesServer.UserEntity", "User")
                        .WithOne("PersonalData")
                        .HasForeignKey("ProfessionalQualitiesServer.PersonalDataEntity", "UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("ProfessionalQualitiesServer.QuestionAnswerOption", b =>
                {
                    b.HasOne("ProfessionalQualitiesServer.AnswerOptionEntity", "AnswerOption")
                        .WithMany()
                        .HasForeignKey("AnswerOptionId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("ProfessionalQualitiesServer.QuestionEntity", "Question")
                        .WithMany("QuestionAnswerOptions")
                        .HasForeignKey("QuestionId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("ProfessionalQualitiesServer.QuestionEntity", b =>
                {
                    b.HasOne("ProfessionalQualitiesServer.TestEntity", "Test")
                        .WithMany("Questions")
                        .HasForeignKey("TestId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("ProfessionalQualitiesServer.ResultEntity", b =>
                {
                    b.HasOne("ProfessionalQualitiesServer.PassedTestEntity", "PassedTest")
                        .WithMany("Results")
                        .HasForeignKey("PassedTestId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("ProfessionalQualitiesServer.ScaleEntity", "Scale")
                        .WithMany()
                        .HasForeignKey("ScaleId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("ProfessionalQualitiesServer.TestAnswerOption", b =>
                {
                    b.HasOne("ProfessionalQualitiesServer.AnswerOptionEntity", "AnswerOption")
                        .WithMany()
                        .HasForeignKey("AnswerOptionId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("ProfessionalQualitiesServer.TestEntity", "Test")
                        .WithMany("TestAnswerOptions")
                        .HasForeignKey("TestId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("ProfessionalQualitiesServer.UserEntity", b =>
                {
                    b.HasOne("ProfessionalQualitiesServer.RoleEntity", "Role")
                        .WithMany("Users")
                        .HasForeignKey("RoleId");
                });
#pragma warning restore 612, 618
        }
    }
}