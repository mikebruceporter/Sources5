using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Sources5.Data.Migrations
{
    public partial class AddNavCountryToUserInfo : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CountryId",
                table: "UserInfos",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_UserInfos_CountryId",
                table: "UserInfos",
                column: "CountryId");


        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {


            migrationBuilder.DropIndex(
                name: "IX_UserInfos_CountryId",
                table: "UserInfos");

            migrationBuilder.DropColumn(
                name: "CountryId",
                table: "UserInfos");
        }
    }
}
