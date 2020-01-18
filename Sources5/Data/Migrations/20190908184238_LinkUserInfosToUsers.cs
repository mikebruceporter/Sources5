using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Sources5.Data.Migrations
{
    public partial class LinkUserInfosToUsers : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ApplicationUserId",
                table: "UserInfos",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserInfos_ApplicationUserId",
                table: "UserInfos",
                column: "ApplicationUserId",
                unique: true,
                filter: "[ApplicationUserId] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_UserInfos_AspNetUsers_ApplicationUserId",
                table: "UserInfos",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserInfos_AspNetUsers_ApplicationUserId",
                table: "UserInfos");

            migrationBuilder.DropIndex(
                name: "IX_UserInfos_ApplicationUserId",
                table: "UserInfos");

            migrationBuilder.DropColumn(
                name: "ApplicationUserId",
                table: "UserInfos");
        }
    }
}
