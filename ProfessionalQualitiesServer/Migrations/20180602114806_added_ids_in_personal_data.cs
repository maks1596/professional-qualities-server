using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace ProfessionalQualitiesServer.Migrations
{
    public partial class added_ids_in_personal_data : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PersonalDataEntity_Professions_ProfessionId",
                table: "PersonalDataEntity");

            migrationBuilder.DropForeignKey(
                name: "FK_Users_PersonalDataEntity_PersonalDataId",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Users_PersonalDataId",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "PersonalDataId",
                table: "Users");

            migrationBuilder.AlterColumn<int>(
                name: "ProfessionId",
                table: "PersonalDataEntity",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "PersonalDataEntity",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_PersonalDataEntity_UserId",
                table: "PersonalDataEntity",
                column: "UserId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_PersonalDataEntity_Professions_ProfessionId",
                table: "PersonalDataEntity",
                column: "ProfessionId",
                principalTable: "Professions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PersonalDataEntity_Users_UserId",
                table: "PersonalDataEntity",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PersonalDataEntity_Professions_ProfessionId",
                table: "PersonalDataEntity");

            migrationBuilder.DropForeignKey(
                name: "FK_PersonalDataEntity_Users_UserId",
                table: "PersonalDataEntity");

            migrationBuilder.DropIndex(
                name: "IX_PersonalDataEntity_UserId",
                table: "PersonalDataEntity");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "PersonalDataEntity");

            migrationBuilder.AddColumn<int>(
                name: "PersonalDataId",
                table: "Users",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "ProfessionId",
                table: "PersonalDataEntity",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.CreateIndex(
                name: "IX_Users_PersonalDataId",
                table: "Users",
                column: "PersonalDataId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_PersonalDataEntity_Professions_ProfessionId",
                table: "PersonalDataEntity",
                column: "ProfessionId",
                principalTable: "Professions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Users_PersonalDataEntity_PersonalDataId",
                table: "Users",
                column: "PersonalDataId",
                principalTable: "PersonalDataEntity",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
