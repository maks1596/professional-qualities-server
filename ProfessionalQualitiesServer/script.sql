CREATE TABLE IF NOT EXISTS "__EFMigrationsHistory" (
    "MigrationId" varchar(150) NOT NULL,
    "ProductVersion" varchar(32) NOT NULL,
    CONSTRAINT "PK___EFMigrationsHistory" PRIMARY KEY ("MigrationId")
);

CREATE TABLE "AnswerOptions" (
    "Id" serial NOT NULL,
    "Formulation" text NULL,
    CONSTRAINT "PK_AnswerOptions" PRIMARY KEY ("Id")
);

CREATE TABLE "Professions" (
    "Id" serial NOT NULL,
    "Name" text NULL,
    CONSTRAINT "PK_Professions" PRIMARY KEY ("Id")
);

INSERT INTO "Professions" ("Name")
VALUES ('╨Я╤А╨╛╨│╤А╨░╨╝╨╝╨╕╤Б╤В');

CREATE TABLE "Roles" (
    "Id" serial NOT NULL,
    "Name" text NULL,
    CONSTRAINT "PK_Roles" PRIMARY KEY ("Id")
);

INSERT INTO "Roles" ("Name")
VALUES ('Admin'),
       ('User');

CREATE TABLE "Scales" (
    "Id" serial NOT NULL,
    "Name" text NULL,
    CONSTRAINT "PK_Scales" PRIMARY KEY ("Id")
);

CREATE TABLE "Tests" (
    "Id" serial NOT NULL,
    "AmountOfVisibleQuestions" int4 NOT NULL,
    "AreAnswerOptionsUnique" bool NOT NULL,
    "Instruction" text NULL,
    "Name" text NULL,
    CONSTRAINT "PK_Tests" PRIMARY KEY ("Id")
);

CREATE TABLE "Users" (
    "Id" serial NOT NULL,
    "Deleted" bool NOT NULL,
    "Login" text NULL,
    "Password" text NULL,
    "RoleId" int4 NULL,
    CONSTRAINT "PK_Users" PRIMARY KEY ("Id"),
    CONSTRAINT "FK_Users_Roles_RoleId" FOREIGN KEY ("RoleId") REFERENCES "Roles" ("Id") ON DELETE RESTRICT
);

INSERT INTO "Users" ("Deleted", "Login", "Password", "RoleId")
VALUES (FALSE, 'admin', 'password', 1);

INSERT INTO "Users" ("Deleted", "Login", "Password", "RoleId")
VALUES (FALSE, 'user', 'password', 2);

CREATE TABLE "EvaluationMaps" (
    "TestId" int4 NOT NULL,
    "ScaleId" int4 NOT NULL,
    "LowerRangeValue" int4 NOT NULL,
    "UpperRangeValue" int4 NOT NULL,
    "Result" text NULL,
    CONSTRAINT "PK_EvaluationMaps" PRIMARY KEY ("TestId", "ScaleId", "LowerRangeValue", "UpperRangeValue"),
    CONSTRAINT "FK_EvaluationMaps_Scales_ScaleId" FOREIGN KEY ("ScaleId") REFERENCES "Scales" ("Id") ON DELETE CASCADE,
    CONSTRAINT "FK_EvaluationMaps_Tests_TestId" FOREIGN KEY ("TestId") REFERENCES "Tests" ("Id") ON DELETE CASCADE
);

CREATE TABLE "Questions" (
    "Id" serial NOT NULL,
    "Formulation" text NULL,
    "Order" int4 NOT NULL,
    "TestId" int4 NULL,
    CONSTRAINT "PK_Questions" PRIMARY KEY ("Id"),
    CONSTRAINT "FK_Questions_Tests_TestId" FOREIGN KEY ("TestId") REFERENCES "Tests" ("Id") ON DELETE CASCADE
);

CREATE TABLE "TestsAnswerOptions" (
    "TestId" int4 NOT NULL,
    "AnswerOptionId" int4 NOT NULL,
    "Order" int4 NOT NULL,
    CONSTRAINT "PK_TestsAnswerOptions" PRIMARY KEY ("TestId", "AnswerOptionId"),
    CONSTRAINT "FK_TestsAnswerOptions_AnswerOptions_AnswerOptionId" FOREIGN KEY ("AnswerOptionId") REFERENCES "AnswerOptions" ("Id") ON DELETE CASCADE,
    CONSTRAINT "FK_TestsAnswerOptions_Tests_TestId" FOREIGN KEY ("TestId") REFERENCES "Tests" ("Id") ON DELETE CASCADE
);

CREATE TABLE "PassedTests" (
    "Id" serial NOT NULL,
    "Date" timestamp NOT NULL,
    "ExpertAsessment" int4 NOT NULL,
    "ProfessionId" int4 NOT NULL,
    "TestId" int4 NOT NULL,
    "TestedId" int4 NOT NULL,
    CONSTRAINT "PK_PassedTests" PRIMARY KEY ("Id"),
    CONSTRAINT "FK_PassedTests_Professions_ProfessionId" FOREIGN KEY ("ProfessionId") REFERENCES "Professions" ("Id") ON DELETE CASCADE,
    CONSTRAINT "FK_PassedTests_Tests_TestId" FOREIGN KEY ("TestId") REFERENCES "Tests" ("Id") ON DELETE CASCADE,
    CONSTRAINT "FK_PassedTests_Users_TestedId" FOREIGN KEY ("TestedId") REFERENCES "Users" ("Id") ON DELETE CASCADE
);

CREATE TABLE "PersonalData" (
    "Id" serial NOT NULL,
    "Birthday" timestamp NOT NULL,
    "ExpertAssessment" int4 NOT NULL,
    "IsMale" bool NOT NULL,
    "Name" text NULL,
    "ProfessionId" int4 NOT NULL,
    "UserId" int4 NOT NULL,
    CONSTRAINT "PK_PersonalData" PRIMARY KEY ("Id"),
    CONSTRAINT "FK_PersonalData_Professions_ProfessionId" FOREIGN KEY ("ProfessionId") REFERENCES "Professions" ("Id") ON DELETE CASCADE,
    CONSTRAINT "FK_PersonalData_Users_UserId" FOREIGN KEY ("UserId") REFERENCES "Users" ("Id") ON DELETE CASCADE
);

CREATE TABLE "Keys" (
    "TestId" int4 NOT NULL,
    "ScaleId" int4 NOT NULL,
    "QuestionId" int4 NOT NULL,
    "AnswerId" int4 NOT NULL,
    "Points" int4 NOT NULL,
    CONSTRAINT "PK_Keys" PRIMARY KEY ("TestId", "ScaleId", "QuestionId", "AnswerId"),
    CONSTRAINT "FK_Keys_AnswerOptions_AnswerId" FOREIGN KEY ("AnswerId") REFERENCES "AnswerOptions" ("Id") ON DELETE CASCADE,
    CONSTRAINT "FK_Keys_Questions_QuestionId" FOREIGN KEY ("QuestionId") REFERENCES "Questions" ("Id") ON DELETE CASCADE,
    CONSTRAINT "FK_Keys_Scales_ScaleId" FOREIGN KEY ("ScaleId") REFERENCES "Scales" ("Id") ON DELETE CASCADE,
    CONSTRAINT "FK_Keys_Tests_TestId" FOREIGN KEY ("TestId") REFERENCES "Tests" ("Id") ON DELETE CASCADE
);

CREATE TABLE "QuestionsAnswerOptions" (
    "QuestionId" int4 NOT NULL,
    "AnswerOptionId" int4 NOT NULL,
    "Order" int4 NOT NULL,
    CONSTRAINT "PK_QuestionsAnswerOptions" PRIMARY KEY ("QuestionId", "AnswerOptionId"),
    CONSTRAINT "FK_QuestionsAnswerOptions_AnswerOptions_AnswerOptionId" FOREIGN KEY ("AnswerOptionId") REFERENCES "AnswerOptions" ("Id") ON DELETE CASCADE,
    CONSTRAINT "FK_QuestionsAnswerOptions_Questions_QuestionId" FOREIGN KEY ("QuestionId") REFERENCES "Questions" ("Id") ON DELETE CASCADE
);

CREATE TABLE "Answers" (
    "Id" serial NOT NULL,
    "AnswerId" int4 NOT NULL,
    "PassedTestId" int4 NOT NULL,
    "QuestionId" int4 NOT NULL,
    CONSTRAINT "PK_Answers" PRIMARY KEY ("Id"),
    CONSTRAINT "FK_Answers_AnswerOptions_AnswerId" FOREIGN KEY ("AnswerId") REFERENCES "AnswerOptions" ("Id") ON DELETE CASCADE,
    CONSTRAINT "FK_Answers_PassedTests_PassedTestId" FOREIGN KEY ("PassedTestId") REFERENCES "PassedTests" ("Id") ON DELETE CASCADE,
    CONSTRAINT "FK_Answers_Questions_QuestionId" FOREIGN KEY ("QuestionId") REFERENCES "Questions" ("Id") ON DELETE CASCADE
);

CREATE TABLE "Results" (
    "PassedTestId" int4 NOT NULL,
    "ScaleId" int4 NOT NULL,
    "Points" int4 NOT NULL,
    "Result" text NULL,
    CONSTRAINT "PK_Results" PRIMARY KEY ("PassedTestId", "ScaleId"),
    CONSTRAINT "FK_Results_PassedTests_PassedTestId" FOREIGN KEY ("PassedTestId") REFERENCES "PassedTests" ("Id") ON DELETE CASCADE,
    CONSTRAINT "FK_Results_Scales_ScaleId" FOREIGN KEY ("ScaleId") REFERENCES "Scales" ("Id") ON DELETE CASCADE
);

CREATE INDEX "IX_Answers_AnswerId" ON "Answers" ("AnswerId");

CREATE INDEX "IX_Answers_PassedTestId" ON "Answers" ("PassedTestId");

CREATE INDEX "IX_Answers_QuestionId" ON "Answers" ("QuestionId");

CREATE INDEX "IX_EvaluationMaps_ScaleId" ON "EvaluationMaps" ("ScaleId");

CREATE INDEX "IX_Keys_AnswerId" ON "Keys" ("AnswerId");

CREATE INDEX "IX_Keys_QuestionId" ON "Keys" ("QuestionId");

CREATE INDEX "IX_Keys_ScaleId" ON "Keys" ("ScaleId");

CREATE INDEX "IX_PassedTests_ProfessionId" ON "PassedTests" ("ProfessionId");

CREATE INDEX "IX_PassedTests_TestId" ON "PassedTests" ("TestId");

CREATE INDEX "IX_PassedTests_TestedId" ON "PassedTests" ("TestedId");

CREATE INDEX "IX_PersonalData_ProfessionId" ON "PersonalData" ("ProfessionId");

CREATE UNIQUE INDEX "IX_PersonalData_UserId" ON "PersonalData" ("UserId");

CREATE INDEX "IX_Questions_TestId" ON "Questions" ("TestId");

CREATE INDEX "IX_QuestionsAnswerOptions_AnswerOptionId" ON "QuestionsAnswerOptions" ("AnswerOptionId");

CREATE INDEX "IX_Results_ScaleId" ON "Results" ("ScaleId");

CREATE INDEX "IX_TestsAnswerOptions_AnswerOptionId" ON "TestsAnswerOptions" ("AnswerOptionId");

CREATE INDEX "IX_Users_RoleId" ON "Users" ("RoleId");

INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
VALUES ('20180608100246_legendary_migration', '2.0.1-rtm-125');

ALTER TABLE "PassedTests" DROP CONSTRAINT "FK_PassedTests_Professions_ProfessionId";

ALTER TABLE "PersonalData" DROP CONSTRAINT "FK_PersonalData_Professions_ProfessionId";

ALTER TABLE "PersonalData" ALTER COLUMN "ProfessionId" TYPE int4;
ALTER TABLE "PersonalData" ALTER COLUMN "ProfessionId" DROP NOT NULL;
ALTER TABLE "PersonalData" ALTER COLUMN "ProfessionId" DROP DEFAULT;

ALTER TABLE "PassedTests" ALTER COLUMN "ProfessionId" TYPE int4;
ALTER TABLE "PassedTests" ALTER COLUMN "ProfessionId" DROP NOT NULL;
ALTER TABLE "PassedTests" ALTER COLUMN "ProfessionId" DROP DEFAULT;

ALTER TABLE "PassedTests" ADD CONSTRAINT "FK_PassedTests_Professions_ProfessionId" FOREIGN KEY ("ProfessionId") REFERENCES "Professions" ("Id") ON DELETE RESTRICT;

ALTER TABLE "PersonalData" ADD CONSTRAINT "FK_PersonalData_Professions_ProfessionId" FOREIGN KEY ("ProfessionId") REFERENCES "Professions" ("Id") ON DELETE RESTRICT;

INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
VALUES ('20180608103215_nullable_profession_foreign_key', '2.0.1-rtm-125');


