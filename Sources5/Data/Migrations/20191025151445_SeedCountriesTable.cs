using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Sources5.Data.Migrations
{
    public partial class SeedCountriesTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("INSERT INTO Countries (Name) VALUES ('Canada')");
            migrationBuilder.Sql("INSERT INTO Countries (Name) VALUES ('USA')");
            migrationBuilder.Sql("INSERT INTO Countries (Name) VALUES ('Tanzania')");
            migrationBuilder.Sql("INSERT INTO Countries (Name) VALUES ('Barbados')");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DELETE FROM Countries");
        }
    }
}
