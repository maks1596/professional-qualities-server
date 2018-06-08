using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace ProfessionalQualitiesServer.Migrations
{
    public partial class nullable_profession_foreign_key : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PassedTests_Professions_ProfessionId",
                table: "PassedTests");

            migrationBuilder.DropForeignKey(
                name: "FK_PersonalData_Professions_ProfessionId",
                table: "PersonalData");

            migrationBuilder.AlterColumn<int>(
                name: "ProfessionId",
                table: "PersonalData",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AlterColumn<int>(
                name: "ProfessionId",
                table: "PassedTests",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AddForeignKey(
                name: "FK_PassedTests_Professions_ProfessionId",
                table: "PassedTests",
                column: "ProfessionId",
                principalTable: "Professions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_PersonalData_Professions_ProfessionId",
                table: "PersonalData",
                column: "ProfessionId",
                principalTable: "Professions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PassedTests_Professions_ProfessionId",
                table: "PassedTests");

            migrationBuilder.DropForeignKey(
                name: "FK_PersonalData_Professions_ProfessionId",
                table: "PersonalData");

            migrationBuilder.AlterColumn<int>(
                name: "ProfessionId",
                table: "PersonalData",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "ProfessionId",
                table: "PassedTests",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_PassedTests_Professions_ProfessionId",
                table: "PassedTests",
                column: "ProfessionId",
                principalTable: "Professions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PersonalData_Professions_ProfessionId",
                table: "PersonalData",
                column: "ProfessionId",
                principalTable: "Professions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
