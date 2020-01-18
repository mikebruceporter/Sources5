using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Sources5.Data.Migrations
{
    public partial class AddOneToManyRelationshipBetweenUserInfosAndProjects : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "UserInfoId",
                table: "Projects",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Projects_UserInfoId",
                table: "Projects",
                column: "UserInfoId");

            migrationBuilder.AddForeignKey(
                name: "FK_Projects_UserInfos_UserInfoId",
                table: "Projects",
                column: "UserInfoId",
                principalTable: "UserInfos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Projects_UserInfos_UserInfoId",
                table: "Projects");

            migrationBuilder.DropIndex(
                name: "IX_Projects_UserInfoId",
                table: "Projects");

            migrationBuilder.DropColumn(
                name: "UserInfoId",
                table: "Projects");
        }
    }
}
