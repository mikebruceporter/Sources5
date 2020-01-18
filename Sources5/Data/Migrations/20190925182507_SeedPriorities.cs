using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Sources5.Data.Migrations
{
    public partial class SeedPriorities : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("INSERT INTO Priorities (Id,Name,Number) VALUES (1,'High',1)");
            migrationBuilder.Sql("INSERT INTO Priorities (Id,Name,Number) VALUES (2,'Medium',2)");
            migrationBuilder.Sql("INSERT INTO Priorities (Id,Name,Number) VALUES (3,'Low',3)");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DELETE FROM Priorities");
        }
    }
}
